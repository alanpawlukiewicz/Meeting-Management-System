import { Component, OnInit, computed, inject, signal } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DayOfWeek, UserAvailabilityDto } from '../../../core/models/availability.model';
import { AvailabilityApiService } from '../../../core/services/api-services/availability-api.service';
import { TimeOffApiService } from '../../../core/services/api-services/time-off-api.service';
import { UserTimeOffAddRequest, UserTimeOffResponse } from '../../../core/models/time-off.models';
import { HttpErrorResponse } from '@angular/common/http';
import { minDateValidator } from '../../../core/validators/min-date.validator';

type WindowGroup = FormGroup<{
  start: FormControl<string>;
  end: FormControl<string>;
}>;

interface WeekDay {
  value: DayOfWeek;
  label: string;
  short: string;
}

/** The week starts on Monday (the enum has Sunday = 0, so the order is explicit). */
const WEEK: WeekDay[] = [
  { value: DayOfWeek.Monday, label: 'Monday', short: 'Mon' },
  { value: DayOfWeek.Tuesday, label: 'Tuesday', short: 'Tue' },
  { value: DayOfWeek.Wednesday, label: 'Wednesday', short: 'Wed' },
  { value: DayOfWeek.Thursday, label: 'Thursday', short: 'Thu' },
  { value: DayOfWeek.Friday, label: 'Friday', short: 'Fri' },
  { value: DayOfWeek.Saturday, label: 'Saturday', short: 'Sat' },
  { value: DayOfWeek.Sunday, label: 'Sunday', short: 'Sun' },
];

const PX_PER_HOUR = 48;
const DEFAULT_RANGE_START_HOUR = 6;
const DEFAULT_RANGE_END_HOUR = 22;

// --- time helpers ---

const pad = (n: number): string => String(n).padStart(2, '0');

/** "08:30:00" -> "08:30" */
const toHm = (time: string): string => time.slice(0, 5);

/** "08:30" -> 510 */
const toMinutes = (time: string): number => {
  const [h, m] = time.split(':').map(Number);
  return h * 60 + m;
};

/** 510 -> "08:30" */
const fromMinutes = (total: number): string => `${pad(Math.floor(total / 60))}:${pad(total % 60)}`;

// --- date helpers (local time only, so DateOnly values never shift because of time zones) ---

/** Date -> "2026-10-22" */
const toDateKey = (d: Date): string => `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}`;

/** "2026-10-22" -> Date at local midnight */
const fromDateKey = (key: string): Date => {
  const [y, m, d] = key.split('-').map(Number);
  return new Date(y, m - 1, d);
};

const addDays = (d: Date, days: number): Date =>
  new Date(d.getFullYear(), d.getMonth(), d.getDate() + days);

/** Monday of the week containing the given date. */
const startOfWeek = (d: Date): Date => addDays(d, -((d.getDay() + 6) % 7));

const formatDateKey = (key: string): string =>
  fromDateKey(key).toLocaleDateString('en-US', {
    weekday: 'short',
    month: 'short',
    day: 'numeric',
    year: 'numeric',
  });

// --- validators ---

function rangeValidator(group: AbstractControl): ValidationErrors | null {
  const { start, end } = group.value as { start: string; end: string };
  return start && end && start >= end ? { range: true } : null;
}

function overlapValidator(array: AbstractControl): ValidationErrors | null {
  const ranges = (array as FormArray<WindowGroup>).controls
    .map((g) => g.getRawValue())
    .filter((r) => r.start && r.end && r.start < r.end)
    .sort((a, b) => a.start.localeCompare(b.start));

  for (let i = 1; i < ranges.length; i++) {
    if (ranges[i].start < ranges[i - 1].end) {
      return { overlap: true };
    }
  }
  return null;
}

@Component({
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatProgressSpinnerModule,
  ],
  selector: 'app-availability-page',
  styleUrl: './availability-page.css',
  templateUrl: './availability-page.html',
})
export class AvailabilityPage implements OnInit {
  private readonly availabilityApi = inject(AvailabilityApiService);
  private readonly timeOffApi = inject(TimeOffApiService);
  private readonly snackBar = inject(MatSnackBar);

  readonly week = WEEK;
  readonly todayKey = toDateKey(new Date());

  // --- state ---
  /** Regular weekly availability. */
  readonly availabilities = signal<UserAvailabilityDto[]>([]);
  /**
   * One-time overrides of the regular availability for a specific date.
   * isAvailable = true  -> free in that window even though the regular schedule says otherwise
   * isAvailable = false -> busy in that window even though the regular schedule says free
   */
  readonly timeOffs = signal<UserTimeOffResponse[]>([]);

  readonly loading = signal(true);
  readonly loadError = signal(false);
  readonly saving = signal(false);
  readonly editing = signal(false);

  readonly weekStart = signal(startOfWeek(new Date()));
  readonly addingTimeOff = signal(false);
  readonly savingTimeOff = signal(false);
  readonly deletingId = signal<string | null>(null);

  // --- forms ---
  /** One FormArray per day of the week (same order as WEEK), each holding that day's time windows. */
  readonly form = new FormArray(
    WEEK.map(() => new FormArray<WindowGroup>([], overlapValidator)),
  );

  /** One-time changes must not overlap each other, regardless of isAvailable. */
  private readonly overlapsExistingTimeOff: ValidatorFn = (group) => {
    const { date, start, end } = group.value as {
      date: string;
      start: string;
      end: string;
    };
    if (!date || !start || !end || start >= end) {
      return null;
    }
    const clash = this.timeOffs().some(
      (t) => t.date === date && start < t.endTime && t.startTime < end,
    );
    return clash ? { timeOffOverlap: true } : null;
  };

  readonly timeOffForm = new FormGroup(
    {
      date: new FormControl('', { nonNullable: true, validators: [Validators.required, minDateValidator(() => this.todayKey)] }),
      start: new FormControl('08:00', { nonNullable: true, validators: [Validators.required] }),
      end: new FormControl('10:00', { nonNullable: true, validators: [Validators.required] }),
      isAvailable: new FormControl(false, { nonNullable: true }),
    },
    { validators: [rangeValidator, this.overlapsExistingTimeOff] },
  );

  // --- week view ---
  readonly range = computed(() => {
    let start = DEFAULT_RANGE_START_HOUR;
    let end = DEFAULT_RANGE_END_HOUR;
    const windows = [...this.availabilities(), ...this.timeOffs()];
    for (const w of windows) {
      start = Math.min(start, Math.floor(toMinutes(w.startTime) / 60));
      end = Math.max(end, Math.ceil(toMinutes(w.endTime) / 60));
    }
    return { start, end };
  });

  readonly gridHeight = computed(() => {
    const { start, end } = this.range();
    return (end - start) * PX_PER_HOUR;
  });

  readonly hours = computed(() => {
    const { start, end } = this.range();
    return Array.from({ length: end - start + 1 }, (_, i) => ({
      label: `${pad(start + i)}:00`,
      top: i * PX_PER_HOUR,
    }));
  });

  readonly weekLabel = computed(() => {
    const start = this.weekStart();
    const end = addDays(start, 6);
    const fmt = (d: Date, withYear: boolean) =>
      d.toLocaleDateString('en-US', {
        month: 'short',
        day: 'numeric',
        ...(withYear ? { year: 'numeric' as const } : {}),
      });
    return `${fmt(start, start.getFullYear() !== end.getFullYear())} – ${fmt(end, true)}`;
  });

  readonly columns = computed(() => {
    const rangeStartMin = this.range().start * 60;
    const weekStart = this.weekStart();
    const availabilities = this.availabilities();
    const timeOffs = this.timeOffs();

    const position = (startTime: string, endTime: string) => {
      const s = toMinutes(startTime);
      const e = toMinutes(endTime);
      return {
        top: ((s - rangeStartMin) / 60) * PX_PER_HOUR,
        height: Math.max(((e - s) / 60) * PX_PER_HOUR, 22),
      };
    };

    return WEEK.map((day, i) => {
      const date = addDays(weekStart, i);
      const dateKey = toDateKey(date);

      return {
        ...day,
        dayNumber: date.getDate(),
        isToday: dateKey === this.todayKey,
        routine: availabilities
          .filter((a) => a.dayOfWeek === day.value)
          .map((a) => ({
            label: `${a.startTime}–${a.endTime}`,
            ...position(a.startTime, a.endTime),
          })),
        overrides: timeOffs
          .filter((t) => t.date === dateKey)
          .map((t) => ({
            id: t.id,
            isAvailable: t.isAvailable,
            label: `${t.isAvailable ? 'Free' : 'Busy'} ${t.startTime}–${t.endTime}`,
            ...position(t.startTime, t.endTime),
          })),
      };
    });
  });

  readonly isEmpty = computed(() => this.availabilities().length === 0);

  // --- one-time changes list ---
  readonly timeOffRows = computed(() =>
    this.timeOffs().map((t) => ({
      id: t.id,
      dateLabel: formatDateKey(t.date),
      timeLabel: `${t.startTime}–${t.endTime}`,
      isAvailable: t.isAvailable,
      isPast: t.date < this.todayKey,
    })),
  );

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.loadError.set(false);

    this.availabilityApi.getAvailabilities().subscribe({
      next: (details) => {
        this.availabilities.set(this.normalizeAvailabilities(details.availabilities));
        this.timeOffs.set(this.sortTimeOffs(details.timeOffs.map((t) => this.normalizeTimeOff(t))));
        this.loading.set(false);
      },
      error: () => {
        this.loadError.set(true);
        this.loading.set(false);
      },
    });
  }

  // --- week navigation ---
  shiftWeek(weeks: number): void {
    this.weekStart.update((d) => addDays(d, weeks * 7));
  }

  goToToday(): void {
    this.weekStart.set(startOfWeek(new Date()));
  }

  // --- editing regular availability ---
  startEditing(): void {
    this.populateForm(this.availabilities());
    this.addingTimeOff.set(false);
    this.editing.set(true);
  }

  cancelEditing(): void {
    this.editing.set(false);
  }

  addWindow(dayIndex: number): void {
    const windows = this.form.at(dayIndex);
    const last = windows.length ? windows.at(windows.length - 1).getRawValue() : null;

    let start = '08:00';
    let end = '10:00';
    if (last?.end) {
      const startMin = toMinutes(last.end);
      start = last.end;
      end = fromMinutes(Math.min(startMin + 120, 23 * 60 + 59));
    }

    windows.push(this.createWindow(start, end));
  }

  removeWindow(dayIndex: number, windowIndex: number): void {
    this.form.at(dayIndex).removeAt(windowIndex);
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    // The backend (TimeOnly) receives "HH:mm:ss"
    const payload: UserAvailabilityDto[] = this.form.controls.flatMap((windows, i) =>
      windows.controls.map((group) => {
        const { start, end } = group.getRawValue();
        return {
          dayOfWeek: WEEK[i].value,
          startTime: `${start}:00`,
          endTime: `${end}:00`,
        };
      }),
    );

    this.saving.set(true);
    this.availabilityApi.updateAvailabilities(payload).subscribe({
      next: (saved) => {
        this.availabilities.set(this.normalizeAvailabilities(saved));
        this.saving.set(false);
        this.editing.set(false);
        this.snackBar.open('Availability saved', undefined, { duration: 3000 });
      },
      error: (err: HttpErrorResponse) => {
        this.saving.set(false);
        this.snackBar.open(err.error['detail'] ?? 'Could not save your availability. Please try again.', 'Close', {
          duration: 5000,
        });
      },
    });
  }

  // --- one-time changes ---
  startAddingTimeOff(): void {
    this.timeOffForm.reset({
      date: this.todayKey,
      start: '08:00',
      end: '10:00',
      isAvailable: false,
    });
    this.addingTimeOff.set(true);
  }

  cancelAddingTimeOff(): void {
    this.addingTimeOff.set(false);
  }

  addTimeOff(): void {
    if (this.timeOffForm.invalid) {
      this.timeOffForm.markAllAsTouched();
      return;
    }

    const { date, start, end, isAvailable } = this.timeOffForm.getRawValue();
    const request: UserTimeOffAddRequest = {
      date, // "YYYY-MM-DD" (DateOnly)
      startTime: `${start}:00`,
      endTime: `${end}:00`,
      isAvailable,
    };

    this.savingTimeOff.set(true);
    this.timeOffApi.addTimeOff(request).subscribe({
      next: (created) => {
        const normalized = this.normalizeTimeOff(created);
        this.timeOffs.update((list) => this.sortTimeOffs([...list, normalized]));
        this.savingTimeOff.set(false);
        this.addingTimeOff.set(false);
        // Show the week that contains the new change
        this.weekStart.set(startOfWeek(fromDateKey(normalized.date)));
        this.snackBar.open('One-time change added', undefined, { duration: 3000 });
      },
      error: (err: HttpErrorResponse) => {
        this.savingTimeOff.set(false);
        this.snackBar.open(
          err.error['detail'] ??
          "Could not add the change. Make sure it doesn't overlap another one-time change.",
          'Close',
          { duration: 5000 },
        );
      },
    });
  }

  deleteTimeOff(id: string): void {
    this.deletingId.set(id);
    this.timeOffApi.deleteTimeOff(id).subscribe({
      next: () => {
        this.timeOffs.update((list) => list.filter((t) => t.id !== id));
        this.deletingId.set(null);
        // If the form is open, its overlap check depends on the list that just changed
        this.timeOffForm.updateValueAndValidity();
        this.snackBar.open('One-time change deleted', undefined, { duration: 3000 });
      },
      error: (err: HttpErrorResponse) => {
        this.deletingId.set(null);
        this.snackBar.open(err.error['detail'] ?? 'Could not delete the change. Please try again.', 'Close', {
          duration: 5000,
        });
      },
    });
  }

  // --- helpers ---
  private normalizeAvailabilities(list: UserAvailabilityDto[]): UserAvailabilityDto[] {
    return list
      .map((a) => ({ ...a, startTime: toHm(a.startTime), endTime: toHm(a.endTime) }))
      .sort((a, b) => a.startTime.localeCompare(b.startTime));
  }

  private normalizeTimeOff(t: UserTimeOffResponse): UserTimeOffResponse {
    return {
      ...t,
      date: t.date.slice(0, 10),
      startTime: toHm(t.startTime),
      endTime: toHm(t.endTime),
    };
  }

  private sortTimeOffs(list: UserTimeOffResponse[]): UserTimeOffResponse[] {
    return [...list].sort(
      (a, b) => a.date.localeCompare(b.date) || a.startTime.localeCompare(b.startTime),
    );
  }

  private populateForm(list: UserAvailabilityDto[]): void {
    this.form.controls.forEach((windows) => windows.clear());

    for (const item of list) {
      const index = WEEK.findIndex((d) => d.value === item.dayOfWeek);
      if (index >= 0) {
        this.form.at(index).push(this.createWindow(item.startTime, item.endTime));
      }
    }
  }

  private createWindow(start: string, end: string): WindowGroup {
    return new FormGroup(
      {
        start: new FormControl(start, { nonNullable: true, validators: [Validators.required] }),
        end: new FormControl(end, { nonNullable: true, validators: [Validators.required] }),
      },
      { validators: rangeValidator },
    );
  }
}
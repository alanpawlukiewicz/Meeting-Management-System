import { Component, OnInit, computed, inject, signal } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DayOfWeek, UserAvailabilityDto } from '../../../core/models/availability.model';
import { AvailabilityApiService } from '../../../core/services/api-services/availability-api.service';

type WindowGroup = FormGroup<{
  start: FormControl<string>;
  end: FormControl<string>;
}>;

interface WeekDay {
  value: DayOfWeek;
  label: string;
  short: string;
}

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

/** "08:30:00" -> "08:30" */
const toHm = (time: string): string => time.slice(0, 5);

/** "08:30" -> 510 */
const toMinutes = (time: string): number => {
  const [h, m] = time.split(':').map(Number);
  return h * 60 + m;
};

/** 510 -> "08:30" */
const fromMinutes = (total: number): string => {
  const h = Math.floor(total / 60);
  const m = total % 60;
  return `${String(h).padStart(2, '0')}:${String(m).padStart(2, '0')}`;
};

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
  private readonly api = inject(AvailabilityApiService);
  private readonly snackBar = inject(MatSnackBar);

  readonly week = WEEK;
  readonly pxPerHour = PX_PER_HOUR;

  // --- state ---
  readonly availabilities = signal<UserAvailabilityDto[]>([]);
  readonly loading = signal(true);
  readonly loadError = signal(false);
  readonly saving = signal(false);
  readonly editing = signal(false);

  /** One FormArray per day of the week (same order as WEEK), each holding that day's time windows. */
  readonly form = new FormArray(
    WEEK.map(() => new FormArray<WindowGroup>([], overlapValidator)),
  );

  // --- week view ---
  readonly range = computed(() => {
    let start = DEFAULT_RANGE_START_HOUR;
    let end = DEFAULT_RANGE_END_HOUR;
    for (const a of this.availabilities()) {
      start = Math.min(start, Math.floor(toMinutes(a.startTime) / 60));
      end = Math.max(end, Math.ceil(toMinutes(a.endTime) / 60));
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
      label: `${String(start + i).padStart(2, '0')}:00`,
      top: i * PX_PER_HOUR,
    }));
  });

  readonly columns = computed(() => {
    const rangeStartMin = this.range().start * 60;
    const all = this.availabilities();

    return WEEK.map((day) => ({
      ...day,
      blocks: all
        .filter((a) => a.dayOfWeek === day.value)
        .map((a) => {
          const s = toMinutes(a.startTime);
          const e = toMinutes(a.endTime);
          return {
            label: `${a.startTime}–${a.endTime}`,
            top: ((s - rangeStartMin) / 60) * PX_PER_HOUR,
            height: Math.max(((e - s) / 60) * PX_PER_HOUR, 22),
          };
        }),
    }));
  });

  readonly isEmpty = computed(() => this.availabilities().length === 0);

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.loadError.set(false);

    this.api.getAvailabilities().subscribe({
      next: (list) => {
        this.availabilities.set(this.normalize(list));
        this.loading.set(false);
      },
      error: () => {
        this.loadError.set(true);
        this.loading.set(false);
      },
    });
  }

  // --- editing ---
  startEditing(): void {
    this.populateForm(this.availabilities());
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
    this.api.updateAvailabilities(payload).subscribe({
      next: (saved) => {
        this.availabilities.set(this.normalize(saved));
        this.saving.set(false);
        this.editing.set(false);
        this.snackBar.open('Availability saved', undefined, { duration: 3000 });
      },
      error: () => {
        this.saving.set(false);
        this.snackBar.open('Could not save your availability. Please try again.', 'Close', {
          duration: 5000,
        });
      },
    });
  }

  // --- helpers ---
  private normalize(list: UserAvailabilityDto[]): UserAvailabilityDto[] {
    return list
      .map((a) => ({ ...a, startTime: toHm(a.startTime), endTime: toHm(a.endTime) }))
      .sort((a, b) => a.startTime.localeCompare(b.startTime));
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
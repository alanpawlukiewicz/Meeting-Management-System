import { Component, computed, inject, signal } from '@angular/core';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import {
  Observable,
  catchError,
  concat,
  debounceTime,
  distinctUntilChanged,
  map,
  of,
  startWith,
  switchMap,
} from 'rxjs';
import { LookupDto } from '../../../core/models/lookup.model';
import {
  UserResponse,
  UserStatusCheckRequest,
  UserStatusCheckResponse,
} from '../../../core/models/user.models';
import { MeetingRequest } from '../../../core/models/meeting.models';
import { MeetingApiService } from '../../../core/services/api-services/meeting-api.service';
import { UserApiService } from '../../../core/services/api-services/user-api.service';
import { UserService } from '../../../core/services/user.service'; // popraw ścieżkę
import { minDateValidator } from '../../../core/validators/min-date.validator';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DATE_LOCALE, provideNativeDateAdapter } from '@angular/material/core';

/** Where to go after the meeting is saved (or when the user cancels). Adjust to your routes. */
const AFTER_SAVE_ROUTE = ['/'];

const SEARCH_DEBOUNCE_MS = 250;
const AVAILABILITY_DEBOUNCE_MS = 400;
const SEARCH_LIMIT = 10;

/**
 * everyone -> participantIds = null (everyone is invited)
 * selected -> participantIds = ids of the chosen users
 */
type ParticipantsMode = 'everyone' | 'selected';

type AvailabilityBadge = 'available' | 'unavailable' | 'checking' | null;

interface SearchState {
  query: string;
  loading: boolean;
  failed: boolean;
  results: LookupDto[];
}

type AvailabilityCheck =
  | { status: 'idle' }
  | { status: 'loading' }
  | { status: 'error' }
  | { status: 'done'; results: UserStatusCheckResponse[] };

const pad = (n: number): string => String(n).padStart(2, '0');

/** Date -> "2026-10-22" (local time, so DateOnly never shifts because of time zones) */
const toDateKey = (d: Date): string => `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}`;

/** "2026-10-22" -> Date (local time) */
const parseDate = (value: string): Date => {
  const [y, m, d] = value.slice(0, 10).split('-').map(Number);
  return new Date(y, m - 1, d);
};

/** "08:30:00" -> "08:30" (value for <input type="time">) */
const toHm = (time: string): string => time.slice(0, 5);

/** UserResponse -> LookupDto used by the participants list (assumes LookupDto = { id, name }) */
const toLookup = (u: UserResponse): LookupDto => ({
  id: u.id,
  name: [u.firstName, u.lastName].filter(Boolean).join(' ') || u.userName,
});

// --- validators ---

function rangeValidator(group: AbstractControl): ValidationErrors | null {
  const { start, end } = group.value as { start: string; end: string };
  return start && end && start >= end ? { range: true } : null;
}

@Component({
  imports: [
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatDatepickerModule
  ],
  selector: 'app-meeting-form-page',
  styleUrl: './meeting-form-page.css',
  templateUrl: './meeting-form-page.html',
  providers: [provideNativeDateAdapter(),
  { provide: MAT_DATE_LOCALE, useValue: 'pl-PL' }
  ],
})
export class MeetingFormPage {
  private readonly meetingApi = inject(MeetingApiService);
  private readonly userApi = inject(UserApiService);
  private readonly userService = inject(UserService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  /** Id in the route => edit mode, no id => create mode. */
  readonly meetingId = this.route.snapshot.paramMap.get('id');
  readonly isEdit = this.meetingId !== null;

  readonly loading = signal(this.isEdit);
  readonly loadError = signal(false);

  readonly todayKey = toDateKey(new Date());
  readonly today = new Date();

  // --- meeting form ---
  readonly form = new FormGroup(
    {
      title: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.maxLength(200)] }),
      place: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
      date: new FormControl<Date>(this.today, {
        nonNullable: true,
        validators: [Validators.required, minDateValidator(() => this.todayKey)],
      }),
      start: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
      end: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
      mode: new FormControl<ParticipantsMode>('everyone', { nonNullable: true }),
    },
    { validators: [rangeValidator] },
  );

  /** The form value as a signal, so computed values can react to it. */
  private readonly formValue = toSignal(
    this.form.valueChanges.pipe(
      startWith(null),
      map(() => this.form.getRawValue()),
    ),
    { requireSync: true },
  );

  readonly mode = computed(() => this.formValue().mode);

  readonly saving = signal(false);
  readonly submitAttempted = signal(false);

  // --- participant search ---
  readonly searchControl = new FormControl<string | LookupDto>('', { nonNullable: true });
  readonly participants = signal<LookupDto[]>([]);

  /** Keeps the input empty after an option is picked (we render the choice in the list instead). */
  readonly clearDisplay = (): string => '';

  readonly searchState = toSignal(
    this.searchControl.valueChanges.pipe(
      // Picking an option emits the LookupDto itself; treat that as an empty query
      map((value) => (typeof value === 'string' ? value.trim() : '')),
      debounceTime(SEARCH_DEBOUNCE_MS),
      distinctUntilChanged(),
      switchMap((query) => this.search(query)),
    ),
    { initialValue: { query: '', loading: false, failed: false, results: [] } as SearchState },
  );

  /** Search results without the people who are already selected. */
  readonly searchOptions = computed(() => {
    const selected = new Set(this.participants().map((p) => p.id));
    return this.searchState().results.filter((u) => !selected.has(u.id));
  });

  readonly noResults = computed(() => {
    const s = this.searchState();
    return s.query.length > 0 && !s.loading && !s.failed && this.searchOptions().length === 0;
  });

  readonly participantsMissing = computed(
    () => this.mode() === 'selected' && this.participants().length === 0,
  );

  // --- availability preview (only for explicitly selected people) ---
  private readonly checkRequest = computed<UserStatusCheckRequest | null>(() => {
    const v = this.formValue();
    const userIds = this.participants().map((p) => p.id);

    if (v.mode !== 'selected' || userIds.length === 0) return null;
    if (!v.date || !v.start || !v.end || v.start >= v.end) return null;

    return { date: toDateKey(v.date), startTime: `${v.start}:00`, endTime: `${v.end}:00`, userIds };
  });

  readonly canPreviewAvailability = computed(() => this.checkRequest() !== null);

  readonly checkState = toSignal(
    toObservable(this.checkRequest).pipe(
      debounceTime(AVAILABILITY_DEBOUNCE_MS),
      distinctUntilChanged((a, b) => JSON.stringify(a) === JSON.stringify(b)),
      switchMap((request): Observable<AvailabilityCheck> => (request ? this.runCheck(request) : of({ status: 'idle' }))),
    ),
    { initialValue: { status: 'idle' } as AvailabilityCheck },
  );

  readonly participantRows = computed(() => {
    const check = this.checkState();
    const byId = new Map<string, boolean>();
    if (check.status === 'done') {
      for (const r of check.results) byId.set(r.userId, r.isAvailable);
    }

    return this.participants().map((p) => {
      let availability: AvailabilityBadge = null;
      if (check.status === 'loading') {
        availability = 'checking';
      } else if (byId.has(p.id)) {
        availability = byId.get(p.id) ? 'available' : 'unavailable';
      }
      return { ...p, availability };
    });
  });

  readonly availabilitySummary = computed(() => {
    const known = this.participantRows().filter(
      (r) => r.availability === 'available' || r.availability === 'unavailable',
    );
    if (this.checkState().status !== 'done' || known.length === 0) return null;
    return {
      available: known.filter((r) => r.availability === 'available').length,
      total: known.length,
    };
  });

  constructor() {
    if (this.meetingId) {
      this.loadMeeting(this.meetingId);
    }
  }

  // --- actions ---
  addParticipant(user: LookupDto): void {
    this.participants.update((list) => (list.some((p) => p.id === user.id) ? list : [...list, user]));
    this.searchControl.setValue('');
  }

  removeParticipant(id: string): void {
    this.participants.update((list) => list.filter((p) => p.id !== id));
  }

  cancel(): void {
    this.router.navigate(AFTER_SAVE_ROUTE);
  }

  submit(): void {
    this.submitAttempted.set(true);

    if (this.form.invalid || this.participantsMissing()) {
      this.form.markAllAsTouched();
      return;
    }

    const { title, place, date, start, end, mode } = this.form.getRawValue();
    const request: MeetingRequest = {
      date: toDateKey(date), // "YYYY-MM-DD" (DateOnly)
      startTime: `${start}:00`, // TimeOnly
      endTime: `${end}:00`,
      place: place.trim(),
      title: title.trim(),
      participantIds:
        mode === 'everyone' ? null : this.participants().map((p) => p.id),
    };

    this.saving.set(true);

    const save$ = this.meetingId
      ? this.meetingApi.updateMeeting(this.meetingId, request)
      : this.meetingApi.addMeeting(request);

    save$.subscribe({
      next: () => {
        this.saving.set(false);
        this.snackBar.open(this.isEdit ? 'Meeting updated' : 'Meeting created', undefined, {
          duration: 3000,
        });
        this.router.navigate(AFTER_SAVE_ROUTE);
      },
      error: () => {
        this.saving.set(false);
        this.snackBar.open(
          this.isEdit
            ? 'Could not update the meeting. Please try again.'
            : 'Could not create the meeting. Please try again.',
          'Close',
          { duration: 5000 },
        );
      },
    });
  }

  // --- helpers ---

  /** Edit mode: finds the meeting in the user's meetings and fills the form. */
  private loadMeeting(id: string): void {
    this.meetingApi.GetUserMeetings().subscribe({
      next: (meetings) => {
        const m = meetings.find((x) => x.id === id);

        if (!m) {
          this.loadError.set(true);
          this.loading.set(false);
          return;
        }

        if (this.userService.currentUser()?.id !== m.creatorId) {
          this.snackBar.open('Only the organizer can edit this meeting.', 'Close', { duration: 5000 });
          this.router.navigate(AFTER_SAVE_ROUTE);
          return;
        }

        this.form.patchValue({
          place: m.place,
          title: m.title,
          date: parseDate(m.date),
          start: toHm(m.startTime),
          end: toHm(m.endTime),
          mode: 'selected',
        });
        this.participants.set(m.participants.filter((p) => p.id !== m.creatorId).map(toLookup));
        this.loading.set(false);
      },
      error: () => {
        this.loadError.set(true);
        this.loading.set(false);
      },
    });
  }

  private search(query: string): Observable<SearchState> {
    if (query.length === 0) {
      return of({ query, loading: false, failed: false, results: [] });
    }

    return concat(
      of<SearchState>({ query, loading: true, failed: false, results: [] }),
      this.userApi.lookupUsers(query, SEARCH_LIMIT).pipe(
        map((results): SearchState => ({ query, loading: false, failed: false, results })),
        catchError(() => of<SearchState>({ query, loading: false, failed: true, results: [] })),
      ),
    );
  }

  private runCheck(request: UserStatusCheckRequest): Observable<AvailabilityCheck> {
    return concat(
      of<AvailabilityCheck>({ status: 'loading' }),
      this.userApi.checkUsersAvailabilities(request).pipe(
        // The endpoint reports one entry per user; accept a single object defensively
        map((res): AvailabilityCheck => ({
          status: 'done',
          results: Array.isArray(res) ? res : [res],
        })),
        catchError(() => of<AvailabilityCheck>({ status: 'error' })),
      ),
    );
  }
}
import { NgTemplateOutlet } from '@angular/common';
import { Component, DestroyRef, OnInit, computed, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AttendanceStatus, MeetingDto } from '../../../core/models/meeting.models';
import { MeetingApiService } from '../../../core/services/api-services/meeting-api.service';

interface MeetingView {
  id: string;
  creatorName: string;
  place: string;
  status: AttendanceStatus;
  statusLabel: string;
  start: Date;
  end: Date;
  startLabel: string;
  endLabel: string;
  isOngoing: boolean;
  isUpdating: boolean;
}

interface MeetingGroup {
  label: string;
  meetings: MeetingView[];
}

const STATUS_LABEL: Record<AttendanceStatus, string> = {
  [AttendanceStatus.Pending]: 'Awaiting your response',
  [AttendanceStatus.Accepted]: 'Accepted',
  [AttendanceStatus.Declined]: 'Declined',
};

const REFRESH_INTERVAL_MS = 60_000;

// --- date helpers (local time only, so DateOnly / TimeOnly values never shift because of time zones) ---

const pad = (n: number): string => String(n).padStart(2, '0');

/** "08:30:00" -> "08:30" */
const toHm = (time: string): string => time.slice(0, 5);

/** ("2026-10-22", "08:30:00") -> Date in local time */
const parseDateTime = (date: string, time: string): Date => {
  const [y, m, d] = date.slice(0, 10).split('-').map(Number);
  const [hh, mm] = time.split(':').map(Number);
  return new Date(y, m - 1, d, hh, mm);
};

const startOfDay = (d: Date): Date => new Date(d.getFullYear(), d.getMonth(), d.getDate());

const toDateKey = (d: Date): string => `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}`;

const dayLabel = (date: Date, now: Date): string => {
  const diffDays = Math.round((startOfDay(date).getTime() - startOfDay(now).getTime()) / 86_400_000);
  if (diffDays === 0) return 'Today';
  if (diffDays === 1) return 'Tomorrow';
  return date.toLocaleDateString('en-US', { weekday: 'long', month: 'short', day: 'numeric' });
};

@Component({
  imports: [
    NgTemplateOutlet,
    MatButtonModule,
    MatButtonToggleModule,
    MatIconModule,
    MatProgressSpinnerModule,
  ],
  selector: 'app-main-page',
  styleUrl: './main-page.css',
  templateUrl: './main-page.html',
})
export class MainPage implements OnInit {
  private readonly meetingApi = inject(MeetingApiService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly destroyRef = inject(DestroyRef);

  /** Exposed so the template can compare against enum members. */
  readonly AttendanceStatus = AttendanceStatus;

  // --- state ---
  private readonly meetings = signal<MeetingDto[]>([]);
  private readonly updatingIds = signal<ReadonlySet<string>>(new Set());
  /** Ticks every minute so "happening now" and "ended" update without a reload. */
  private readonly now = signal(new Date());

  readonly loading = signal(true);
  readonly loadError = signal(false);

  // --- derived ---
  /** Ongoing and upcoming meetings (finished ones are dropped), sorted by start. */
  private readonly items = computed<MeetingView[]>(() => {
    const now = this.now();
    const updating = this.updatingIds();

    return this.meetings()
      .map((dto): MeetingView => {
        const start = parseDateTime(dto.date, dto.startTime);
        let end = parseDateTime(dto.date, dto.endTime);
        if (end <= start) {
          // A meeting that runs past midnight ends on the next day
          end = new Date(end.getFullYear(), end.getMonth(), end.getDate() + 1, end.getHours(), end.getMinutes());
        }

        const statusLabel = dto.status == AttendanceStatus.Pending && start <= now && now < end ? "Did not respond" : STATUS_LABEL[dto.status];

        return {
          id: dto.id,
          creatorName: dto.creatorName,
          place: dto.place,
          status: dto.status,
          statusLabel: statusLabel,
          start,
          end,
          startLabel: toHm(dto.startTime),
          endLabel: toHm(dto.endTime),
          isOngoing: start <= now && now < end,
          isUpdating: updating.has(dto.id),
        };
      })
      .filter((m) => m.end > now)
      .sort((a, b) => a.start.getTime() - b.start.getTime());
  });

  readonly ongoing = computed(() => this.items().filter((m) => m.isOngoing));

  readonly upcomingGroups = computed<MeetingGroup[]>(() => {
    const now = this.now();
    const groups = new Map<string, MeetingGroup>();

    for (const m of this.items().filter((item) => !item.isOngoing)) {
      const key = toDateKey(m.start);
      let group = groups.get(key);
      if (!group) {
        group = { label: dayLabel(m.start, now), meetings: [] };
        groups.set(key, group);
      }
      group.meetings.push(m);
    }

    return [...groups.values()];
  });

  readonly pendingCount = computed(
    () => this.items().filter((m) => m.status === AttendanceStatus.Pending && !m.isOngoing).length,
  );

  readonly isEmpty = computed(() => this.items().length === 0);

  ngOnInit(): void {
    this.load();

    const timer = setInterval(() => this.now.set(new Date()), REFRESH_INTERVAL_MS);
    this.destroyRef.onDestroy(() => clearInterval(timer));
  }

  load(): void {
    this.loading.set(true);
    this.loadError.set(false);

    this.meetingApi.GetUserMeetings().subscribe({
      next: (meetings) => {
        this.meetings.set(meetings);
        this.now.set(new Date());
        this.loading.set(false);
      },
      error: () => {
        this.loadError.set(true);
        this.loading.set(false);
      },
    });
  }

  /** Optimistic update: the UI changes immediately and is rolled back if the request fails. */
  changeStatus(meetingId: string, status: AttendanceStatus): void {
    const previous = this.meetings().find((m) => m.id === meetingId)?.status;
    if (previous === undefined || previous === status) {
      return;
    }

    this.applyStatus(meetingId, status);
    this.updatingIds.update((ids) => new Set(ids).add(meetingId));

    this.meetingApi.changeAttendanceStatus(meetingId, status).subscribe({
      next: () => this.finishUpdate(meetingId),
      error: () => {
        this.applyStatus(meetingId, previous);
        this.finishUpdate(meetingId);
        this.snackBar.open('Could not update your response. Please try again.', 'Close', {
          duration: 5000,
        });
      },
    });
  }

  private applyStatus(meetingId: string, status: AttendanceStatus): void {
    this.meetings.update((list) => list.map((m) => (m.id === meetingId ? { ...m, status } : m)));
  }

  private finishUpdate(meetingId: string): void {
    this.updatingIds.update((ids) => {
      const next = new Set(ids);
      next.delete(meetingId);
      return next;
    });
  }
}
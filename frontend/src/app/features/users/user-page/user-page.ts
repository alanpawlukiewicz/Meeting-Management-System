import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserDto } from '../../../core/models/user.models';
import { UserApiService } from '../../../core/services/api-services/user-api.service';
import { Role } from '../../../core/models/auth.models';

type UserFilter = 'all' | 'organizers' | 'others';

const isOrganizerRole = (role: Role): boolean => role === Role.Organizer;

const initialsOf = (user: UserDto): string => {
  const letters = `${user.firstName.charAt(0)}${user.lastName.charAt(0)}`.trim();
  return (letters || user.userName.charAt(0)).toUpperCase();
};

@Component({
  imports: [
    MatButtonModule,
    MatButtonToggleModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatSlideToggleModule,
  ],
  selector: 'app-user-page',
  styleUrl: './user-page.css',
  templateUrl: './user-page.html',
})
export class UserPage implements OnInit {
  private readonly userApi = inject(UserApiService);
  private readonly snackBar = inject(MatSnackBar);

  // --- state ---
  private readonly users = signal<UserDto[]>([]);
  private readonly organizerOverrides = signal<ReadonlyMap<string, boolean>>(new Map());
  private readonly updatingIds = signal<ReadonlySet<string>>(new Set());

  readonly loading = signal(true);
  readonly loadError = signal(false);
  readonly query = signal('');
  readonly filter = signal<UserFilter>('all');

  // --- derived ---
  readonly rows = computed(() => {
    const overrides = this.organizerOverrides();
    const updating = this.updatingIds();

    return this.users()
      .map((user) => ({
        id: user.id,
        fullName: `${user.firstName} ${user.lastName}`.trim(),
        userName: user.userName,
        initials: initialsOf(user),
        isOrganizer: overrides.get(user.id) ?? user.roleNames.some(isOrganizerRole),
        otherRoles: user.roleNames.filter((r) => !isOrganizerRole(r)).map(String),
        isUpdating: updating.has(user.id),
        sortKey: `${user.lastName} ${user.firstName}`.toLowerCase(),
      }))
      .sort((a, b) => a.sortKey.localeCompare(b.sortKey));
  });

  readonly filteredRows = computed(() => {
    const query = this.query().trim().toLowerCase();
    const filter = this.filter();

    return this.rows().filter((row) => {
      if (filter === 'organizers' && !row.isOrganizer) return false;
      if (filter === 'others' && row.isOrganizer) return false;
      return (
        query === '' ||
        row.fullName.toLowerCase().includes(query) ||
        row.userName.toLowerCase().includes(query)
      );
    });
  });

  readonly organizerCount = computed(() => this.rows().filter((r) => r.isOrganizer).length);

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.loadError.set(false);

    this.userApi.getUsersWithRoles().subscribe({
      next: (users) => {
        this.users.set(users);
        this.organizerOverrides.set(new Map());
        this.loading.set(false);
      },
      error: () => {
        this.loadError.set(true);
        this.loading.set(false);
      },
    });
  }

  onSearch(event: Event): void {
    this.query.set((event.target as HTMLInputElement).value);
  }

  /** Optimistic update: the toggle changes immediately and is rolled back if the request fails. */
  setOrganizer(userId: string, grant: boolean): void {
    const row = this.rows().find((r) => r.id === userId);
    if (!row || row.isOrganizer === grant) {
      return;
    }

    const previousOverride = this.organizerOverrides().get(userId);
    this.updateOverride(userId, grant);
    this.updatingIds.update((ids) => new Set(ids).add(userId));

    const request$ = grant
      ? this.userApi.giveOrganizerRole(userId)
      : this.userApi.revokeOrganizerRole(userId);

    request$.subscribe({
      next: () => {
        this.finishUpdate(userId);
        this.snackBar.open(
          grant
            ? `${row.fullName} is now an organizer`
            : `Organizer role revoked from ${row.fullName}`,
          undefined,
          { duration: 3000 },
        );
      },
      error: () => {
        this.updateOverride(userId, previousOverride);
        this.finishUpdate(userId);
        this.snackBar.open(
          `Could not ${grant ? 'grant' : 'revoke'} the organizer role. Please try again.`,
          'Close',
          { duration: 5000 },
        );
      },
    });
  }

  /** Sets the override, or removes it when `value` is undefined. */
  private updateOverride(userId: string, value: boolean | undefined): void {
    this.organizerOverrides.update((current) => {
      const next = new Map(current);
      if (value === undefined) {
        next.delete(userId);
      } else {
        next.set(userId, value);
      }
      return next;
    });
  }

  private finishUpdate(userId: string): void {
    this.updatingIds.update((ids) => {
      const next = new Set(ids);
      next.delete(userId);
      return next;
    });
  }
}
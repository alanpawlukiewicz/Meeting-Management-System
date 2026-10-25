import { HttpErrorResponse } from '@angular/common/http';
import { Component, computed, effect, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import {
  AbstractControl,
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
import { map, startWith } from 'rxjs';
import { AccountApiService } from '../../../core/services/api-services/account-api.service';
import { AuthService } from '../../../core/services/auth.service';
import { UserService } from '../../../core/services/user.service';
import { UserUpdateRequest } from '../../../core/models/user.models';
import { UserProfile } from '../../../core/models/auth.models';
import { usernameTakenValidator } from '../../../core/validators/username-taken.validator';
import { UserApiService } from '../../../core/services/api-services/user-api.service';

const HTTP_CONFLICT = 409;

// --- validators ---

function notBlank(control: AbstractControl): ValidationErrors | null {
  return typeof control.value === 'string' && control.value.trim() === '' ? { required: true } : null;
}

function passwordsValidator(group: AbstractControl): ValidationErrors | null {
  const { oldPassword, newPassword, confirmPassword } = group.value as {
    oldPassword: string;
    newPassword: string;
    confirmPassword: string;
  };

  const errors: ValidationErrors = {};
  if (newPassword && confirmPassword && newPassword !== confirmPassword) {
    errors['mismatch'] = true;
  }
  if (oldPassword && newPassword && oldPassword === newPassword) {
    errors['sameAsOld'] = true;
  }
  return Object.keys(errors).length ? errors : null;
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
  selector: 'app-settings-page',
  styleUrl: './settings-page.css',
  templateUrl: './settings-page.html',
})
export class SettingsPage {
  private readonly accountApi = inject(AccountApiService);
  private readonly auth = inject(AuthService);
  private readonly userService = inject(UserService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly userApiService = inject(UserApiService);

  /** The signed-in user (null until UserService.loadCurrentUser() has finished). */
  readonly user = this.userService.currentUser;

  // --- profile ---
  readonly profileForm = new FormGroup({
    userName: new FormControl('', {
      nonNullable: true, validators: [notBlank],
      asyncValidators: [usernameTakenValidator(this.userApiService, () => this.user()?.userName)],
      updateOn: 'blur',
    }),
    firstName: new FormControl('', { nonNullable: true }),
    lastName: new FormControl('', { nonNullable: true }),
  });

  private readonly profileValue = toSignal(
    this.profileForm.valueChanges.pipe(
      startWith(null),
      map(() => this.profileForm.getRawValue()),
    ),
    { requireSync: true },
  );

  /**
   * Only the fields that actually changed are sent; unchanged fields are null.
   * Null when nothing changed.
   */
  private readonly profileRequest = computed<UserUpdateRequest | null>(() => {
    const user = this.user();
    if (!user) return null;

    const value = this.profileValue();
    const userName = value.userName.trim();
    const firstName = value.firstName.trim();
    const lastName = value.lastName.trim();

    const request: UserUpdateRequest = {
      userName: userName !== user.userName ? userName : null,
      firstName: firstName !== (user.firstName ?? '') ? firstName : null,
      lastName: lastName !== (user.lastName ?? '') ? lastName : null,
    };

    return request.userName === null && request.firstName === null && request.lastName === null
      ? null
      : request;
  });

  readonly hasProfileChanges = computed(() => this.profileRequest() !== null);
  readonly savingProfile = signal(false);

  // --- password ---
  readonly passwordForm = new FormGroup(
    {
      oldPassword: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
      newPassword: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/), Validators.minLength(8)] }),
      confirmPassword: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    },
    { validators: [passwordsValidator] },
  );

  readonly showPasswords = signal(false);
  readonly savingPassword = signal(false);

  // --- delete account ---
  readonly confirmingDelete = signal(false);
  readonly deleteConfirmation = signal('');
  readonly deleting = signal(false);

  readonly canDelete = computed(() => {
    const user = this.user();
    return !!user && this.deleteConfirmation().trim() === user.userName;
  });

  constructor() {
    // Fill the form once the user is available (and again if it changes while the form is untouched)
    effect(() => {
      const user = this.user();
      if (user && this.profileForm.pristine) {
        this.fillProfile(user);
      }
    });
  }

  // --- profile actions ---
  resetProfile(): void {
    const user = this.user();
    if (user) this.fillProfile(user);
  }

  saveProfile(): void {
    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      return;
    }

    const request = this.profileRequest();
    if (!request) return;

    this.savingProfile.set(true);
    this.accountApi.updateUserProfile(request).subscribe({
      next: (updated) => {
        this.userService.setUser(updated);
        this.fillProfile(updated);
        this.savingProfile.set(false);
        this.snackBar.open('Profile updated', undefined, { duration: 3000 });
      },
      error: (error: HttpErrorResponse) => {
        this.savingProfile.set(false);
        this.snackBar.open('Could not update your profile. Please try again.', 'Close', {
          duration: 5000,
        });
      },
    });
  }

  // --- password actions ---
  togglePasswordVisibility(): void {
    this.showPasswords.update((visible) => !visible);
  }

  changePassword(): void {
    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();
      return;
    }

    const { oldPassword, newPassword } = this.passwordForm.getRawValue();

    this.savingPassword.set(true);
    this.auth.changeUserPassword({ oldPassword, newPassword }).subscribe({
      next: () => {
        this.passwordForm.reset();
        this.showPasswords.set(false);
        this.savingPassword.set(false);
        this.snackBar.open('Password changed', undefined, { duration: 3000 });
      },
      error: () => {
        this.savingPassword.set(false);
        this.snackBar.open(
          'Could not change your password. Check your current password and try again.',
          'Close',
          { duration: 5000 },
        );
      },
    });
  }

  // --- delete account actions ---
  startDeleting(): void {
    this.deleteConfirmation.set('');
    this.confirmingDelete.set(true);
  }

  cancelDeleting(): void {
    this.confirmingDelete.set(false);
  }

  onDeleteConfirmationInput(event: Event): void {
    this.deleteConfirmation.set((event.target as HTMLInputElement).value);
  }

  deleteAccount(): void {
    if (!this.canDelete()) return;

    this.deleting.set(true);
    this.accountApi.deleteCurrentUser().subscribe({
      next: () => {
        this.deleting.set(false);
        this.snackBar.open('Your account has been deleted', undefined, { duration: 5000 });
        // Clears the token and the stored user, then navigates to /login
        this.auth.endSession();
      },
      error: () => {
        this.deleting.set(false);
        this.snackBar.open('Could not delete your account. Please try again.', 'Close', {
          duration: 5000,
        });
      },
    });
  }

  // --- helpers ---
  /** Resets the form to the given user and marks it pristine. */
  private fillProfile(user: Pick<UserProfile, 'userName' | 'firstName' | 'lastName'>): void {
    this.profileForm.reset({
      userName: user.userName,
      firstName: user.firstName ?? '',
      lastName: user.lastName ?? '',
    });
  }
}
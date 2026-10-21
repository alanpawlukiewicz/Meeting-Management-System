import { Component, DestroyRef, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AuthService } from '../../../core/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { RegisterRequest } from '../../../core/models/auth.models';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { passwordMatchValidator } from '../../../core/validators/password-match.validator';

@Component({
  imports: [MatFormFieldModule,
    MatCardModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    RouterLink
  ],
  selector: 'app-register-page',
  styleUrl: './register-page.css',
  templateUrl: './register-page.html',
})
export class RegisterPage {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly destroyRef = inject(DestroyRef);
  private readonly router = inject(Router);

  registerErrorSignal = signal<string | null>(null);

  registerForm = this.fb.group({
    firstName: [''],
    lastName: [''],
    userName: ['', Validators.required],
    password: ['', [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/), Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required, passwordMatchValidator('password')]],
  },
  );

  onSubmit() {
    if (this.registerForm.invalid) {
      return;
    }
    const request: RegisterRequest = {
      password: this.registerForm.controls.password.getRawValue() ?? '',
      userName: this.registerForm.controls.userName.getRawValue() ?? '',
      firstName: this.registerForm.controls.firstName.getRawValue(),
      lastName: this.registerForm.controls.lastName.getRawValue()
    };
    this.authService.register(request)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        error: (err: HttpErrorResponse) => {
          this.registerErrorSignal.set(err.error["detail"]);
        }
      });
  }
}

import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs';
import { catchError, debounceTime, first, map, switchMap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { UserApiService } from '../services/api-services/user-api.service';

export function usernameTakenValidator(userService: UserApiService,
  currentUserName: () => string | undefined): AsyncValidatorFn {
  return (control: AbstractControl): ReturnType<AsyncValidatorFn> => {
    const value = (control.value ?? '').trim();

    if (!value || value === currentUserName()) {
      return of(null);
    }

    return of(value).pipe(
      debounceTime(400),
      switchMap((username) =>
        userService.checkUserNameAvailability(username)
      ),
      map((isAvailable): ValidationErrors | null =>
        isAvailable ? null : { usernameTaken: true }
      ),
      catchError(() => of(null)),
      first()
    );
  };
}
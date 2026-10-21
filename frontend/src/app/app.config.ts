import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { AuthService } from './core/services/auth.service';
import { UserService } from './core/services/user.service';
import { catchError, firstValueFrom, of, switchMap, timeout } from 'rxjs';
import { authInterceptor } from './core/interceptors/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideAppInitializer(() => {
      const authService = inject(AuthService);
      const userService = inject(UserService);

      return firstValueFrom(
        authService.refreshToken().pipe(
          switchMap(() => userService.loadCurrentUser()),
          timeout(5_000),
          catchError(() => {
            authService.clearToken();
            userService.clearUser();
            return of(null);
          }),
        ),
      );
    })
  ]
};

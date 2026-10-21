import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { AuthService } from '../services/auth.service';
import { Role } from '../models/auth.models';

export const authGuard = (requiredRole: Role | null = null): CanActivateFn => {
  return (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);
    const userService = inject(UserService);

    const token: string | null = authService.getToken()

    if (requiredRole === null && token) {
      return true;
    }

    if (requiredRole == Role.Admin && userService.isAdministrator() && token)
    {
        return true;
    }

    return router.createUrlTree(['/login'], {
      queryParams: { returnUrl: state.url }
    });
  };
};
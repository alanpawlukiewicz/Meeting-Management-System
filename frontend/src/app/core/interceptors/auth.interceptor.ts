import {
    HttpInterceptorFn,
    HttpErrorResponse,
    HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError, map, of } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { environment } from '../../../environments/environment';

const AUTH_ENDPOINTS = ['/auth/login', '/auth/refresh', '/auth/register'];

const withToken = (req: HttpRequest<unknown>, token: string | null) =>
    token ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : req;

export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const authService = inject(AuthService);

    const isApiCall = req.url.startsWith(environment.apiUrl);
    const isAuthEndpoint = AUTH_ENDPOINTS.some((path) => req.url.includes(path));

    if (!isApiCall || isAuthEndpoint) {
        return next(req);
    }

    const token$ =
        authService.getToken() && authService.isTokenExpiringSoon()
            ? authService.refreshToken().pipe(map((model) => model.token))
            : of(authService.getToken());

    return token$.pipe(
        switchMap((token) => next(withToken(req, token))),
        catchError((error: unknown) => {
            const isUnauthorized = error instanceof HttpErrorResponse && error.status === 401;

            if (!isUnauthorized || !authService.getToken()) {
                return throwError(() => error);
            }

            return authService.refreshToken().pipe(
                switchMap((model) => next(withToken(req, model.token))),
                catchError((refreshError: unknown) => {
                    authService.endSession();
                    return throwError(() => refreshError);
                }),
            );
        }),
    );
};
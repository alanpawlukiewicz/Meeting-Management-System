import { HttpClient } from '@angular/common/http';
import { inject, Service, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { catchError, finalize, map, Observable, of, shareReplay, switchMap, tap } from 'rxjs';
import { Router } from '@angular/router';
import { TokenModel } from '../models/token.model';
import { UserService } from './user.service';
import { LoginRequest, UserProfile } from '../models/auth.models';

@Service()
export class AuthService {
    private readonly http = inject(HttpClient);
    private readonly router = inject(Router);
    private readonly userService = inject(UserService);
    private readonly apiUrl = environment.apiUrl;

    private readonly accessToken = signal<string | null>(null);
    private expiresAt: number | null = null;
    private refreshTimer?: ReturnType<typeof setTimeout>;
    private refreshInFlight$: Observable<TokenModel> | null = null;

    private static readonly REFRESH_BEFORE_EXPIRY_MS = 60_000;
    private static readonly MIN_REFRESH_DELAY_MS = 5_000;

    getToken(): string | null {
        return this.accessToken();
    }

    isTokenExpiringSoon(skewMs = 30_000): boolean {
        return this.expiresAt !== null && this.expiresAt - Date.now() <= skewMs;
    }

    setToken(model: TokenModel): void {
        this.accessToken.set(model.token);
        this.expiresAt = new Date(model.expirationDate).getTime();
        this.scheduleRefresh();
    }

    clearToken(): void {
        clearTimeout(this.refreshTimer);
        this.accessToken.set(null);
        this.expiresAt = null;
    }
    
    refreshToken(): Observable<TokenModel> {
        this.refreshInFlight$ ??= this.http
            .post<TokenModel>(`${this.apiUrl}/auth/refresh`, {}, { withCredentials: true })
            .pipe(
                tap((model) => this.setToken(model)),
                finalize(() => (this.refreshInFlight$ = null)),
                shareReplay({ bufferSize: 1, refCount: false }),
            );

        return this.refreshInFlight$;
    }

    login(loginRequest: LoginRequest): Observable<UserProfile> {
        return this.http
            .post<TokenModel>(`${this.apiUrl}/auth/login`, loginRequest, { withCredentials: true })
            .pipe(
                tap((model) => this.setToken(model)),
                switchMap(() => this.userService.loadCurrentUser()),
                tap(() => this.router.navigate(['/home'])),
            );
    }

    logout(): Observable<void> {
        return this.http
            .post<void>(`${this.apiUrl}/auth/logout`, {}, { withCredentials: true })
            .pipe(
                catchError(() => of(void 0)),
                tap(() => this.endSession()),
                map(() => void 0),
            );
    }

    endSession(): void {
        this.clearToken();
        this.userService.clearUser();
        this.router.navigate(['/login']);
    }

    private scheduleRefresh(): void {
        clearTimeout(this.refreshTimer);
        if (this.expiresAt === null) return;

        const delay = Math.max(
            this.expiresAt - Date.now() - AuthService.REFRESH_BEFORE_EXPIRY_MS,
            AuthService.MIN_REFRESH_DELAY_MS,
        );

        this.refreshTimer = setTimeout(() => {
            this.refreshToken().subscribe({ error: () => this.endSession() });
        }, delay);
    }
}
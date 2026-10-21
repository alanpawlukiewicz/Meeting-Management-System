import { HttpClient } from '@angular/common/http';
import { computed, inject, Service, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable, tap } from 'rxjs';
import { Role, UserProfile } from '../models/auth.models';

@Service()
export class UserService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = environment.apiUrl;

    private readonly currentUserSignal = signal<UserProfile | null>(null);
    readonly currentUser = this.currentUserSignal.asReadonly();

    readonly isLoggedIn = computed(() => this.currentUser() !== null);

    readonly isAdministrator = computed(
        () => this.currentUserSignal()?.roles.includes(Role.Admin) ?? false,
    );

    loadCurrentUser(): Observable<UserProfile> {
        return this.http
            .get<UserProfile>(`${this.apiUrl}/account/me`)
            .pipe(tap((user) => this.currentUserSignal.set(user)));
    }

    clearUser(): void {
        this.currentUserSignal.set(null);
    }
}
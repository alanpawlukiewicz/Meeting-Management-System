import { inject, Service } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { UserResponse, UserUpdateRequest } from '../../models/user.models';
import { Observable } from 'rxjs';

@Service()
export class AccountApiService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/accounts`;

    updateUserProfile(request: UserUpdateRequest): Observable<UserResponse> {
        return this.http.put<UserResponse>(`${this.apiUrl}/me`, request);
    }

    deleteCurrentUser(): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/me`);
    }
}

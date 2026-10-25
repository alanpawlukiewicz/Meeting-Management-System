import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { UserDto, UserStatusCheckRequest, UserStatusCheckResponse } from '../../models/user.models';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { LookupDto } from '../../models/lookup.model';

@Service()
export class UserApiService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/users`;
    
    checkUsersAvailabilities(request: UserStatusCheckRequest): Observable<UserStatusCheckResponse> {
        return this.http.post<UserStatusCheckResponse>(`${this.apiUrl}/availabilities`, request);
    }

    lookupUsers(query: string, limit: number = 10): Observable<LookupDto[]> {
        const params = new HttpParams().set('q', query).set('limit', limit);
        return this.http.get<LookupDto[]>(`${this.apiUrl}/lookup`, { params });
    }

    getUsersWithRoles(): Observable<UserDto[]> {
        return this.http.get<UserDto[]>(this.apiUrl);
    }

    giveOrganizerRole(userId: string): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/${userId}/roles/organizer`, {})
    }

    revokeOrganizerRole(userId: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${userId}/roles/organizer`, {})
    }

    checkUserNameAvailability(username: string): Observable<boolean> {
        const params = new HttpParams().set('username', username);
        return this.http.get<boolean>(`${this.apiUrl}/availability`, { params });
    }
}

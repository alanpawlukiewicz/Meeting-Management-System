import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { UserStatusCheckRequest, UserStatusCheckResponse } from '../../models/user.models';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { LookupDto } from '../../models/lookup.model';

@Service()
export class UserApiService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = environment.apiUrl;
    
    checkUsersAvailabilities(request: UserStatusCheckRequest): Observable<UserStatusCheckResponse> {
        return this.http.post<UserStatusCheckResponse>(`${this.apiUrl}/user/availabilities`, request);
    }

    lookupUsers(query: string, limit: number = 10): Observable<LookupDto[]> {
        const params = new HttpParams().set('q', query).set('limit', limit);
        return this.http.get<LookupDto[]>(`${this.apiUrl}/user`, { params });
    }
}

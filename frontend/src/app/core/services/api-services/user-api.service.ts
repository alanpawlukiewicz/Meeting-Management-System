import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { UserStatusCheckRequest, UserStatusCheckResponse } from '../../models/user.models';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Service()
export class UserApiService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = environment.apiUrl;
    
    checkUsersAvailabilities(request: UserStatusCheckRequest): Observable<UserStatusCheckResponse> {
        return this.http.post<UserStatusCheckResponse>(`${this.apiUrl}/user/availabilities`, request);
    }
}

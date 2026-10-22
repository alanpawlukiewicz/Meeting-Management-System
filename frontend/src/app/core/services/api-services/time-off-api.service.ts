import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { UserTimeOffAddRequest, UserTimeOffResponse } from '../../models/time-off.models';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Service()
export class TimeOffApiService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = environment.apiUrl;

    addTimeOff(request: UserTimeOffAddRequest): Observable<UserTimeOffResponse> {
        return this.http.post<UserTimeOffResponse>(`${this.apiUrl}/time-off/me`, request);
    }

    deleteTimeOff(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/time-off/${id}`);
    }
}

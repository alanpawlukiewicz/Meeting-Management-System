import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { UserAvailabilityDto } from '../../models/availability.model';
import { Observable } from 'rxjs';

@Service()
export class AvailabilityApiService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = environment.apiUrl;

    updateAvailabilities(reguestList: UserAvailabilityDto[]): Observable<UserAvailabilityDto[]> {
        return this.http.put<UserAvailabilityDto[]>(`${this.apiUrl}/availability/me`, reguestList, { withCredentials: true });
    }

    getAvailabilities(): Observable<UserAvailabilityDto[]> {
        return this.http.get<UserAvailabilityDto[]>(`${this.apiUrl}/availability/me`, { withCredentials: true });
    }
}

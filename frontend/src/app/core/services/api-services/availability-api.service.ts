import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { UserAvailabilityDetails, UserAvailabilityDto } from '../../models/availability.model';
import { Observable } from 'rxjs';

@Service()
export class AvailabilityApiService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/availabilities`;

    updateAvailabilities(reguestList: UserAvailabilityDto[]): Observable<UserAvailabilityDto[]> {
        return this.http.put<UserAvailabilityDto[]>(`${this.apiUrl}/me`, reguestList);
    }

    getAvailabilities(): Observable<UserAvailabilityDetails> {
        return this.http.get<UserAvailabilityDetails>(`${this.apiUrl}/me`);
    }
}

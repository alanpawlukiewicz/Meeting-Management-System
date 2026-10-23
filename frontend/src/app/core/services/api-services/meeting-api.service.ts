import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { AttendanceStatus, MeetingDto } from '../../models/meeting.models';
import { environment } from '../../../../environments/environment';

@Service()
export class MeetingApiService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = environment.apiUrl;

    GetUserMeetings(): Observable<MeetingDto[]> {
        return this.http.get<MeetingDto[]>(`${this.apiUrl}/meeting/me`);
    }

    changeAttendanceStatus(meetingId: string, status: AttendanceStatus): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/meeting/${meetingId}/attendance`, status);
    }
}

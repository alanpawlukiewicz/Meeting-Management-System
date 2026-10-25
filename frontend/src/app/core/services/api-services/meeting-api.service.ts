import { HttpClient } from '@angular/common/http';
import { inject, Service } from '@angular/core';
import { Observable } from 'rxjs';
import { AttendanceStatus, MeetingDto, MeetingRequest, MeetingResponse } from '../../models/meeting.models';
import { environment } from '../../../../environments/environment';

@Service()
export class MeetingApiService {
    private readonly http = inject(HttpClient);
    private readonly apiUrl = `${environment.apiUrl}/meetings`;

    GetUserMeetings(): Observable<MeetingDto[]> {
        return this.http.get<MeetingDto[]>(`${this.apiUrl}/me`);
    }

    changeAttendanceStatus(meetingId: string, status: AttendanceStatus): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/${meetingId}/attendance`, {status: status});
    }

    addMeeting(request: MeetingRequest): Observable<MeetingResponse> {
        return this.http.post<MeetingResponse>(this.apiUrl, request);
    }

    updateMeeting(meetingId: string, request: MeetingRequest): Observable<MeetingResponse> {
        return this.http.put<MeetingResponse>(`${this.apiUrl}/${meetingId}`, request);
    }
}

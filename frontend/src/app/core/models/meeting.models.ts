export interface MeetingDto {
    id: string;
    creatorName: string;
    date: string;
    startTime: string;
    endTime: string;
    place: string;
    status: AttendanceStatus;
}

export enum AttendanceStatus {
    Pending = "Pending", Accepted = "Accepted", Declined = "Declined"
}

export interface MeetingRequest {
    date: string;
    startTime: string;
    endTime: string;
    place: string;
    participantIds: string[] | null;
}

export interface MeetingResponse {
    id: string;
    date: string;
    startTime: string;
    endTime: string;
    place: string;
}
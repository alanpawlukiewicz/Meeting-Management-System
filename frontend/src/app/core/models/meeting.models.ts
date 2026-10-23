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
    Pending, Accepted, Declined
}
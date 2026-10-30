import { UserResponse } from "./user.models";

export interface MeetingDto {
    id: string;
    creatorId: string;
    creatorName: string;
    date: string;
    startTime: string;
    endTime: string;
    place: string;
    title: string;
    status: AttendanceStatus;
    participants: UserResponse[];
}

export enum AttendanceStatus {
    Pending = "Pending", Accepted = "Accepted", Declined = "Declined"
}

export interface MeetingRequest {
    date: string;
    startTime: string;
    endTime: string;
    place: string;
    title: string;
    participantIds: string[] | null;
}

export interface MeetingResponse {
    id: string;
    date: string;
    startTime: string;
    endTime: string;
    place: string;
    title: string;
}
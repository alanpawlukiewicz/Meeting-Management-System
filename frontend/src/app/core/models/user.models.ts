import { Role } from "./auth.models";

export interface UserStatusCheckRequest {
    date: string;
    startTime: string;
    endTime: string;
    userIds: string[];
}

export interface UserStatusCheckResponse {
    userId: string;
    userName: string;
    isAvailable: boolean;
}

export interface UserDto {
    id: string;
    userName: string;
    firstName: string;
    lastName: string;
    roleNames: Role[];
}
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

export interface UserUpdateRequest {
    userName: string | null;
    firstName: string | null;
    lastName: string | null;
}

export interface UserResponse {
    id: string;
    userName: string;
    firstName: string | null;
    lastName: string | null;
}
export interface LoginRequest {
    userName: string;
    password: string;
}

export interface UserProfile {
    id: string;
    userName: string;
    firstName: string | null;
    lastName: string | null;
    roles: Role[];
}

export enum Role {
    Admin = "Admin", Organizer = "Organizer"
}

export interface RegisterRequest {
    userName: string;
    firstName: string | null;
    lastName: string | null;
    password: string;
}

export interface PasswordUpdateRequest {
    oldPassword: string;
    newPassword: string;
}
export interface LoginRequest {
    userName: string;
    password: string;
}

export interface UserProfile {
    userName: string;
    firstName: string;
    lastName: string;
    roles: Role[];
}

export enum Role {
    Admin
}
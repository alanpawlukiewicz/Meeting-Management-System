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
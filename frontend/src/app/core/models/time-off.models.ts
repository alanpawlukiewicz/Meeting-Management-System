export interface UserTimeOffAddRequest {
    date: string;
    startTime: string;
    endTime: string;
    isAvailable: boolean;
}

export interface UserTimeOffResponse {
    id: string;
    date: string;
    startTime: string;
    endTime: string;
    isAvailable: boolean;
}
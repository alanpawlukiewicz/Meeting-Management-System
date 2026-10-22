export interface UserAvailabilityDto {
    dayOfWeek: DayOfWeek;
    startTime: string;
    endTime: string;
}

export enum DayOfWeek {
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6,
    Sunday = 0
}
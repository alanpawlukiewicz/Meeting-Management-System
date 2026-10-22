import { UserTimeOffResponse } from "./time-off.models";

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

export interface UserAvailabilityDetails {
    availabilities: UserAvailabilityDto[];
    timeOffs: UserTimeOffResponse[];
}
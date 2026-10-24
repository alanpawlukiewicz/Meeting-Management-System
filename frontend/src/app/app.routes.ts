import { Routes } from '@angular/router';
import { LoginPage } from './features/auth/login-page/login-page';
import { RegisterPage } from './features/auth/register-page/register-page';
import { MainPage } from './features/home/main-page/main-page';
import { NotFoundPage } from './features/error/not-found-page/not-found-page';
import { authGuard } from './core/guards/auth.guard';
import { guestGuard } from './core/guards/guest.guard';
import { AvailabilityPage } from './features/availability/availability-page/availability-page';
import { Role } from './core/models/auth.models';
import { AddMeetingPage } from './features/meeting/add-meeting-page/add-meeting-page';

export const routes: Routes = [
    {path: 'home', component: MainPage, canActivate: [authGuard()]},
    {path: 'availability', component: AvailabilityPage, canActivate: [authGuard()]},
    {path: 'login', component: LoginPage, canActivate: [guestGuard]},
    {path: 'register', component: RegisterPage, canActivate: [guestGuard]},
    {path: 'meeting/add', component: AddMeetingPage, canActivate: [authGuard(Role.Organizer)]},
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: '**', component: NotFoundPage },
];

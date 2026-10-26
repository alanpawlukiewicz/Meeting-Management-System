import { Routes } from '@angular/router';
import { LoginPage } from './features/auth/login-page/login-page';
import { RegisterPage } from './features/auth/register-page/register-page';
import { MainPage } from './features/home/main-page/main-page';
import { NotFoundPage } from './features/error/not-found-page/not-found-page';
import { authGuard } from './core/guards/auth.guard';
import { guestGuard } from './core/guards/guest.guard';
import { AvailabilityPage } from './features/availability/availability-page/availability-page';
import { Role } from './core/models/auth.models';
import { MeetingFormPage } from './features/meeting/add-meeting-page/meeting-form-page';
import { UserPage } from './features/users/user-page/user-page';
import { SettingsPage } from './features/users/settings-page/settings-page';

export const routes: Routes = [
    {path: 'home', component: MainPage, canActivate: [authGuard()]},
    {path: 'availability', component: AvailabilityPage, canActivate: [authGuard()]},
    {path: 'login', component: LoginPage, canActivate: [guestGuard]},
    {path: 'register', component: RegisterPage, canActivate: [guestGuard]},
    {path: 'meetings/add', component: MeetingFormPage, canActivate: [authGuard(Role.Organizer)]},
    {path: 'meetings/:id/edit', component: MeetingFormPage, canActivate: [authGuard(Role.Organizer)]},
    {path: 'users', component: UserPage, canActivate: [authGuard(Role.Admin)]},
    {path: 'settings', component: SettingsPage, canActivate: [authGuard()]},
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: '**', component: NotFoundPage },
];

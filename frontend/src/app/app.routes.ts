import { Routes } from '@angular/router';
import { LoginPage } from './features/auth/login-page/login-page';
import { RegisterPage } from './features/auth/register-page/register-page';
import { MainPage } from './features/home/main-page/main-page';
import { NotFoundPage } from './features/error/not-found-page/not-found-page';

export const routes: Routes = [
    {path: 'home', component: MainPage},
    {path: 'login', component: LoginPage},
    {path: 'register', component: RegisterPage},
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: '**', component: NotFoundPage },
];

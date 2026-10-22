import { Component, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbar } from '@angular/material/toolbar';
import { RouterLink, RouterOutlet } from '@angular/router';
import { UserService } from './core/services/user.service';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, 
    MatIconModule, 
    MatToolbar, 
    MatButtonModule, 
    MatTooltipModule,
  RouterLink],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  readonly userService = inject(UserService);
  readonly authService = inject(AuthService);

  logOut() {
    this.authService.logout().subscribe();
  }
}

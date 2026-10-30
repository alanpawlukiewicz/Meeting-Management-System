import { Component, inject, input, model, output, signal } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MeetingView } from '../../home/main-page/main-page';
import { AttendanceStatus } from '../../../core/models/meeting.models';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatButtonModule } from '@angular/material/button';

@Component({
  imports: [MatIconModule, MatButtonToggleModule, MatButtonModule],
  selector: 'app-meeting-card',
  styleUrl: './meeting-card.css',
  templateUrl: './meeting-card.html',
})
export class MeetingCard {
  meeting = model.required<MeetingView>();
  onDelete = output<void>();
  onUpdate = output<void>();
  onStatusChange = output<AttendanceStatus>();

  confirmDelete = signal<boolean>(false);

  readonly AttendanceStatus = AttendanceStatus;

  askDelete() {
    this.confirmDelete.set(true);
  }

  cancelDelete() {
    this.confirmDelete.set(false);
  }

  deleteMeeting() {
    this.confirmDelete.set(false);
    this.onDelete.emit();
  }
}

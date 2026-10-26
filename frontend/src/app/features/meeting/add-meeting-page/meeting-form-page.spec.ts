import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MeetingFormPage } from './meeting-form-page';

describe('AddMeetingPage', () => {
  let component: MeetingFormPage;
  let fixture: ComponentFixture<MeetingFormPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MeetingFormPage],
    }).compileComponents();

    fixture = TestBed.createComponent(MeetingFormPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

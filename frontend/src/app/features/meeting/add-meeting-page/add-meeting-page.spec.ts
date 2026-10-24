import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddMeetingPage } from './add-meeting-page';

describe('AddMeetingPage', () => {
  let component: AddMeetingPage;
  let fixture: ComponentFixture<AddMeetingPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddMeetingPage],
    }).compileComponents();

    fixture = TestBed.createComponent(AddMeetingPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MeetingCard } from './meeting-card';

describe('MeetingCard', () => {
  let component: MeetingCard;
  let fixture: ComponentFixture<MeetingCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MeetingCard],
    }).compileComponents();

    fixture = TestBed.createComponent(MeetingCard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { MeetingApiService } from './meeting-api-service';

describe('MeetingApiService', () => {
  let service: MeetingApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MeetingApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

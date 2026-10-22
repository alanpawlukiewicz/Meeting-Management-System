import { TestBed } from '@angular/core/testing';
import { TimeOffApiService } from './time-off-api.service'

describe('TimeOffApi', () => {
  let service: TimeOffApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TimeOffApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

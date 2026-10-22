import { TestBed } from '@angular/core/testing';
import { AvailabilityApi } from './availability-api';

describe('AvailabilityApi', () => {
  let service: AvailabilityApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AvailabilityApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

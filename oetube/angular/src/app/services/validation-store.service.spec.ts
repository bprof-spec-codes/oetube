import { TestBed } from '@angular/core/testing';

import { ValidationStoreService } from './validation-store.service';

describe('ValidationStoreService', () => {
  let service: ValidationStoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ValidationStoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

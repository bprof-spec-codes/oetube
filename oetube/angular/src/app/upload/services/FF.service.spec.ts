import { TestBed } from '@angular/core/testing';

import { FFService } from './FF.service';

describe('FFService', () => {
  let service: FFService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FFService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { TranscoderService } from './transcoder.service';

describe('TranscoderService', () => {
  let service: TranscoderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TranscoderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

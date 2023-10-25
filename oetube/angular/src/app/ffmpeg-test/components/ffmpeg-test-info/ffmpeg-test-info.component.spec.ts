import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FfmpegTestInfoComponent } from './ffmpeg-test-info.component';

describe('FfmpegTestInfoComponent', () => {
  let component: FfmpegTestInfoComponent;
  let fixture: ComponentFixture<FfmpegTestInfoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FfmpegTestInfoComponent]
    });
    fixture = TestBed.createComponent(FfmpegTestInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

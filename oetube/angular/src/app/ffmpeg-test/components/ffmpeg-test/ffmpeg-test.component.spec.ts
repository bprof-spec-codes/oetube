import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FfmpegTestComponent } from './ffmpeg-test.component';

describe('FfmpegTestComponent', () => {
  let component: FfmpegTestComponent;
  let fixture: ComponentFixture<FfmpegTestComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FfmpegTestComponent]
    });
    fixture = TestBed.createComponent(FfmpegTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

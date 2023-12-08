import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoFrameSelectorComponent } from './video-frame-selector.component';

describe('VideoFrameSelectorComponent', () => {
  let component: VideoFrameSelectorComponent;
  let fixture: ComponentFixture<VideoFrameSelectorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VideoFrameSelectorComponent]
    });
    fixture = TestBed.createComponent(VideoFrameSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

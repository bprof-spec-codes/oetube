import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoSeekerComponent } from './video-seeker.component';

describe('VideoSeekerComponent', () => {
  let component: VideoSeekerComponent;
  let fixture: ComponentFixture<VideoSeekerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VideoSeekerComponent]
    });
    fixture = TestBed.createComponent(VideoSeekerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

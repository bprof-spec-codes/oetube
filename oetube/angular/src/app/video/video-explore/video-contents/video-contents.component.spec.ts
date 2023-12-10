import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoContentsComponent } from './video-contents.component';

describe('VideoContentsComponent', () => {
  let component: VideoContentsComponent;
  let fixture: ComponentFixture<VideoContentsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VideoContentsComponent]
    });
    fixture = TestBed.createComponent(VideoContentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

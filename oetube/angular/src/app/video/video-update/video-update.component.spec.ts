import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoUpdateComponent } from './video-update.component';

describe('VideoUpdateComponent', () => {
  let component: VideoUpdateComponent;
  let fixture: ComponentFixture<VideoUpdateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VideoUpdateComponent]
    });
    fixture = TestBed.createComponent(VideoUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

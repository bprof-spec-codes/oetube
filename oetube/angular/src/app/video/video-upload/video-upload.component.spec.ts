import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoUploadComponent } from './video-upload.component';

describe('VideoUploadComponenet', () => {
  let component: VideoUploadComponent;
  let fixture: ComponentFixture<VideoUploadComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VideoUploadComponent]
    });
    fixture = TestBed.createComponent(VideoUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoGridComponent } from './video-grid.component';

describe('VideoGridComponent', () => {
  let component: VideoGridComponent;
  let fixture: ComponentFixture<VideoGridComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VideoGridComponent]
    });
    fixture = TestBed.createComponent(VideoGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

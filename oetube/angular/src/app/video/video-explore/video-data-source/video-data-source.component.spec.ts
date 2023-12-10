import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoDataSourceComponent } from './video-data-source.component';

describe('VideoDataSourceComponent', () => {
  let component: VideoDataSourceComponent;
  let fixture: ComponentFixture<VideoDataSourceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VideoDataSourceComponent]
    });
    fixture = TestBed.createComponent(VideoDataSourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

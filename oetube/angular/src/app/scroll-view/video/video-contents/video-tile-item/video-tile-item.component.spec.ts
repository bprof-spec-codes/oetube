import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoTileItemComponent } from './video-tile-item.component';

describe('VideoTileItemComponent', () => {
  let component: VideoTileItemComponent;
  let fixture: ComponentFixture<VideoTileItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VideoTileItemComponent]
    });
    fixture = TestBed.createComponent(VideoTileItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

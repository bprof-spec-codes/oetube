import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistVideoDataSourceComponent } from './playlist-video-data-source.component';

describe('PlaylistVideoDataSourceComponent', () => {
  let component: PlaylistVideoDataSourceComponent;
  let fixture: ComponentFixture<PlaylistVideoDataSourceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistVideoDataSourceComponent]
    });
    fixture = TestBed.createComponent(PlaylistVideoDataSourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

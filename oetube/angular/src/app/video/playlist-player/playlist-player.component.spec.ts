import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistPlayerComponent } from './playlist-player.component';

describe('PlaylistPlayerComponent', () => {
  let component: PlaylistPlayerComponent;
  let fixture: ComponentFixture<PlaylistPlayerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistPlayerComponent]
    });
    fixture = TestBed.createComponent(PlaylistPlayerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

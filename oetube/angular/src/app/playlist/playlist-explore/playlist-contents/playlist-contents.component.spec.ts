import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistContentsComponent } from './playlist-contents.component';

describe('PlaylistContentsComponent', () => {
  let component: PlaylistContentsComponent;
  let fixture: ComponentFixture<PlaylistContentsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistContentsComponent]
    });
    fixture = TestBed.createComponent(PlaylistContentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

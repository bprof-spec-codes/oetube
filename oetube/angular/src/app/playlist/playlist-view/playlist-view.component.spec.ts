import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistViewComponent } from './playlist-view.component';

describe('PlaylistViewComponent', () => {
  let component: PlaylistViewComponent;
  let fixture: ComponentFixture<PlaylistViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistViewComponent]
    });
    fixture = TestBed.createComponent(PlaylistViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

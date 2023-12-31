import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistListItemComponent } from './playlist-list-item.component';

describe('PlaylistListItemComponent', () => {
  let component: PlaylistListItemComponent;
  let fixture: ComponentFixture<PlaylistListItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistListItemComponent]
    });
    fixture = TestBed.createComponent(PlaylistListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

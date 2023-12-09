import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistYourListsComponent } from './playlist-your-lists.component';

describe('PlaylistYourListsComponent', () => {
  let component: PlaylistYourListsComponent;
  let fixture: ComponentFixture<PlaylistYourListsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistYourListsComponent]
    });
    fixture = TestBed.createComponent(PlaylistYourListsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

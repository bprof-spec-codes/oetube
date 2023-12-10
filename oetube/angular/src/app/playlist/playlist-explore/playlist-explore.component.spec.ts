import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistExploreComponent } from './playlist-explore.component';

describe('PlaylistExploreComponent', () => {
  let component: PlaylistExploreComponent;
  let fixture: ComponentFixture<PlaylistExploreComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistExploreComponent]
    });
    fixture = TestBed.createComponent(PlaylistExploreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

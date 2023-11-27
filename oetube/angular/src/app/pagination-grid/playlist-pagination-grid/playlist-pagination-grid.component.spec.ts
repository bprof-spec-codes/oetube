import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistPaginationGridComponent } from './playlist-pagination-grid.component';

describe('PlaylistPaginationGridComponent', () => {
  let component: PlaylistPaginationGridComponent;
  let fixture: ComponentFixture<PlaylistPaginationGridComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistPaginationGridComponent]
    });
    fixture = TestBed.createComponent(PlaylistPaginationGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

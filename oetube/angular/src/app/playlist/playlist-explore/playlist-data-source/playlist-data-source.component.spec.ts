import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistDataSourceComponent } from './playlist-data-source.component';

describe('PlaylistDataSourceComponent', () => {
  let component: PlaylistDataSourceComponent;
  let fixture: ComponentFixture<PlaylistDataSourceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistDataSourceComponent]
    });
    fixture = TestBed.createComponent(PlaylistDataSourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

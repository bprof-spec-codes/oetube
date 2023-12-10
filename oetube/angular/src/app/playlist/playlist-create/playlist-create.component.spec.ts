import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistCreateComponent } from './playlist-create.component';

describe('PlaylistCreateComponent', () => {
  let component: PlaylistCreateComponent;
  let fixture: ComponentFixture<PlaylistCreateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistCreateComponent]
    });
    fixture = TestBed.createComponent(PlaylistCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

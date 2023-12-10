import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistEditorComponent } from './playlist-editor.component';

describe('PlaylistEditorComponent', () => {
  let component: PlaylistEditorComponent;
  let fixture: ComponentFixture<PlaylistEditorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PlaylistEditorComponent]
    });
    fixture = TestBed.createComponent(PlaylistEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupEditorComponent } from './group-editor.component';

describe('GroupCreateComponent', () => {
  let component: GroupEditorComponent;
  let fixture: ComponentFixture<GroupEditorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GroupEditorComponent]
    });
    fixture = TestBed.createComponent(GroupEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

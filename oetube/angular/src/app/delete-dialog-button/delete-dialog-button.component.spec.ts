import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteDialogButtonComponent } from './delete-dialog-button.component';

describe('DeleteDialogButtonComponent', () => {
  let component: DeleteDialogButtonComponent;
  let fixture: ComponentFixture<DeleteDialogButtonComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DeleteDialogButtonComponent]
    });
    fixture = TestBed.createComponent(DeleteDialogButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

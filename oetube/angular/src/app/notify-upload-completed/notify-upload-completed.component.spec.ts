import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotifyUploadCompletedComponent } from './notify-upload-completed.component';

describe('NotifyUploadCompletedComponent', () => {
  let component: NotifyUploadCompletedComponent;
  let fixture: ComponentFixture<NotifyUploadCompletedComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NotifyUploadCompletedComponent]
    });
    fixture = TestBed.createComponent(NotifyUploadCompletedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

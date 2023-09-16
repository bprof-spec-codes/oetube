import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DevExtremeComponent } from './dev-extreme.component';

describe('DevExtremeComponent', () => {
  let component: DevExtremeComponent;
  let fixture: ComponentFixture<DevExtremeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DevExtremeComponent]
    });
    fixture = TestBed.createComponent(DevExtremeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

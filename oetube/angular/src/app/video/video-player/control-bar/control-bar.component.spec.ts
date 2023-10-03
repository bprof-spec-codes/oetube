import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlBarComponent } from './control-bar.component';

describe('ControlBarComponent', () => {
  let component: ControlBarComponent;
  let fixture: ComponentFixture<ControlBarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ControlBarComponent]
    });
    fixture = TestBed.createComponent(ControlBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

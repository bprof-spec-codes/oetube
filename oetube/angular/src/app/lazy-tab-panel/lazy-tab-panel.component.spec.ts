import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LazyTabPanelComponent } from './lazy-tab-panel.component';

describe('LazyTabPanelComponent', () => {
  let component: LazyTabPanelComponent;
  let fixture: ComponentFixture<LazyTabPanelComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LazyTabPanelComponent]
    });
    fixture = TestBed.createComponent(LazyTabPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

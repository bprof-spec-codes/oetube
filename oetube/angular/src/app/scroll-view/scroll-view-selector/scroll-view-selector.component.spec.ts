import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScrollViewSelectorComponent } from './scroll-view-selector.component';

describe('ScrollViewSelectorComponent', () => {
  let component: ScrollViewSelectorComponent;
  let fixture: ComponentFixture<ScrollViewSelectorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ScrollViewSelectorComponent]
    });
    fixture = TestBed.createComponent(ScrollViewSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

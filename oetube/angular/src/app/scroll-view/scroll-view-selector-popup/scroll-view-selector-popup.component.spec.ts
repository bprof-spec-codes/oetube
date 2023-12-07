import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScrollViewSelectorPopupComponent } from './scroll-view-selector-popup.component';

describe('LazyPopupScrollViewSelectorComponent', () => {
  let component: ScrollViewSelectorPopupComponent;
  let fixture: ComponentFixture<ScrollViewSelectorPopupComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ScrollViewSelectorPopupComponent]
    });
    fixture = TestBed.createComponent(ScrollViewSelectorPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

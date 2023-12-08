import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScrollViewContentsComponent } from './scroll-view-contents.component';

describe('ScrollViewContentsComponent', () => {
  let component: ScrollViewContentsComponent;
  let fixture: ComponentFixture<ScrollViewContentsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ScrollViewContentsComponent]
    });
    fixture = TestBed.createComponent(ScrollViewContentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

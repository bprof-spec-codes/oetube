import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScrollViewDataSourceComponent } from './scroll-view-data-source.component';

describe('ScrollViewDataSourceComponent', () => {
  let component: ScrollViewDataSourceComponent;
  let fixture: ComponentFixture<ScrollViewDataSourceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ScrollViewDataSourceComponent]
    });
    fixture = TestBed.createComponent(ScrollViewDataSourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

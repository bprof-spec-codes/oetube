import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DateSearchItemComponent } from './date-search-item.component';

describe('DateSearchItemComponent', () => {
  let component: DateSearchItemComponent;
  let fixture: ComponentFixture<DateSearchItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DateSearchItemComponent]
    });
    fixture = TestBed.createComponent(DateSearchItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DurationSearchItemComponent } from './duration-search-item.component';

describe('DurationSearchItemComponent', () => {
  let component: DurationSearchItemComponent;
  let fixture: ComponentFixture<DurationSearchItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DurationSearchItemComponent]
    });
    fixture = TestBed.createComponent(DurationSearchItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

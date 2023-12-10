import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextSearchItemComponent } from './text-search-item.component';

describe('TextSearchItemComponent', () => {
  let component: TextSearchItemComponent;
  let fixture: ComponentFixture<TextSearchItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TextSearchItemComponent]
    });
    fixture = TestBed.createComponent(TextSearchItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

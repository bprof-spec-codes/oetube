import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DropDownSearchComponent } from './drop-down-search.component';

describe('DropDownSearchComponent', () => {
  let component: DropDownSearchComponent;
  let fixture: ComponentFixture<DropDownSearchComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DropDownSearchComponent]
    });
    fixture = TestBed.createComponent(DropDownSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupSearchComponent } from './group-search.component';

describe('GroupSearchComponent', () => {
  let component: GroupSearchComponent;
  let fixture: ComponentFixture<GroupSearchComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GroupSearchComponent]
    });
    fixture = TestBed.createComponent(GroupSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

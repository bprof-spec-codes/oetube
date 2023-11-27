import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupPaginationGridComponent } from './group-pagination-grid.component';

describe('GroupPaginationGridComponent', () => {
  let component: GroupPaginationGridComponent;
  let fixture: ComponentFixture<GroupPaginationGridComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GroupPaginationGridComponent]
    });
    fixture = TestBed.createComponent(GroupPaginationGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

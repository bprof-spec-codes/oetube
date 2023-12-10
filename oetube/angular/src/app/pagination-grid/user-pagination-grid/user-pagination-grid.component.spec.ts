import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserPaginationGridComponent } from './user-pagination-grid.component';

describe('UserPaginationGridComponent', () => {
  let component: UserPaginationGridComponent;
  let fixture: ComponentFixture<UserPaginationGridComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserPaginationGridComponent]
    });
    fixture = TestBed.createComponent(UserPaginationGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

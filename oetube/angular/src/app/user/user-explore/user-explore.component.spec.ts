import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserExploreComponent } from './user-explore.component';

describe('UserExploreComponent', () => {
  let component: UserExploreComponent;
  let fixture: ComponentFixture<UserExploreComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserExploreComponent]
    });
    fixture = TestBed.createComponent(UserExploreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

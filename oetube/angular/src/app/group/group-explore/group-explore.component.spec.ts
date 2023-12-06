import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupExploreComponent } from './group-explore.component';

describe('GroupExploreComponent', () => {
  let component: GroupExploreComponent;
  let fixture: ComponentFixture<GroupExploreComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GroupExploreComponent]
    });
    fixture = TestBed.createComponent(GroupExploreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupDataSourceComponent } from './group-data-source.component';

describe('GroupDataSourceComponent', () => {
  let component: GroupDataSourceComponent;
  let fixture: ComponentFixture<GroupDataSourceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GroupDataSourceComponent]
    });
    fixture = TestBed.createComponent(GroupDataSourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

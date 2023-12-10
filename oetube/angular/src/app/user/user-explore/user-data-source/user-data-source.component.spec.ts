import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDataSourceComponent } from './user-data-source.component';

describe('UserDataSourceComponent', () => {
  let component: UserDataSourceComponent;
  let fixture: ComponentFixture<UserDataSourceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserDataSourceComponent]
    });
    fixture = TestBed.createComponent(UserDataSourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

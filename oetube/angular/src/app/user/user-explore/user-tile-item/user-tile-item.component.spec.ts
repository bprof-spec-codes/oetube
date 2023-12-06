import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserTileItemComponent } from './user-tile-item.component';

describe('UserTileItemComponent', () => {
  let component: UserTileItemComponent;
  let fixture: ComponentFixture<UserTileItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [UserTileItemComponent]
    });
    fixture = TestBed.createComponent(UserTileItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

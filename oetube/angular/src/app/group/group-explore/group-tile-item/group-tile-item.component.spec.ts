import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupTileItemComponent } from './group-tile-item.component';

describe('GroupTileItemComponent', () => {
  let component: GroupTileItemComponent;
  let fixture: ComponentFixture<GroupTileItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GroupTileItemComponent]
    });
    fixture = TestBed.createComponent(GroupTileItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

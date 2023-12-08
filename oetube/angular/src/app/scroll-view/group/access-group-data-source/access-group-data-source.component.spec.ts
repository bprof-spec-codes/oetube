import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccessGroupDataSourceComponent } from './access-group-data-source.component';

describe('AccessGroupDataSourceComponent', () => {
  let component: AccessGroupDataSourceComponent;
  let fixture: ComponentFixture<AccessGroupDataSourceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccessGroupDataSourceComponent]
    });
    fixture = TestBed.createComponent(AccessGroupDataSourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

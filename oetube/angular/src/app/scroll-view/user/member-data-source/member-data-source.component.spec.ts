import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberDataSourceComponent } from './member-data-source.component';

describe('MemberDataSourceComponent', () => {
  let component: MemberDataSourceComponent;
  let fixture: ComponentFixture<MemberDataSourceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MemberDataSourceComponent]
    });
    fixture = TestBed.createComponent(MemberDataSourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

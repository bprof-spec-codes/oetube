import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoPaginationGridComponent } from './video-pagination-grid.component';

describe('VideoPaginationGridComponent', () => {
  let component: VideoPaginationGridComponent;
  let fixture: ComponentFixture<VideoPaginationGridComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VideoPaginationGridComponent]
    });
    fixture = TestBed.createComponent(VideoPaginationGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

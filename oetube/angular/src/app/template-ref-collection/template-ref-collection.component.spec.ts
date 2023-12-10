import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemplateRefCollectionComponent } from './template-ref-collection.component';

describe('TemplateRefCollectionComponent', () => {
  let component: TemplateRefCollectionComponent;
  let fixture: ComponentFixture<TemplateRefCollectionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TemplateRefCollectionComponent]
    });
    fixture = TestBed.createComponent(TemplateRefCollectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

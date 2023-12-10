import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppTemplateDirective, TemplateRefCollectionComponent, } from './template-ref-collection.component';



@NgModule({
  declarations: [TemplateRefCollectionComponent, AppTemplateDirective],
  imports: [
    CommonModule
  ],
  exports:[TemplateRefCollectionComponent,AppTemplateDirective]
})
export class TemplateRefCollectionModule { }

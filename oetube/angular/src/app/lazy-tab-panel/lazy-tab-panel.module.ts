import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LazyTabPanelComponent } from './lazy-tab-panel.component';
import { DxTabPanelModule } from 'devextreme-angular';
import { TemplateRefCollectionModule } from '../template-ref-collection/template-ref-collection.module';



@NgModule({
  declarations: [LazyTabPanelComponent],
  imports: [
    CommonModule,
    DxTabPanelModule,
  ],
  exports:[LazyTabPanelComponent,TemplateRefCollectionModule]
})
export class LazyTabPanelModule { }

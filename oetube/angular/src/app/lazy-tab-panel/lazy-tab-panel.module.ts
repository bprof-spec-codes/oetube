import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LazyTabItemDirective, LazyTabPanelComponent } from './lazy-tab-panel.component';
import { DxTabPanelModule } from 'devextreme-angular';



@NgModule({
  declarations: [LazyTabPanelComponent,LazyTabItemDirective],
  imports: [
    CommonModule,
    DxTabPanelModule,
  ],
  exports:[LazyTabPanelComponent,LazyTabItemDirective]
})
export class LazyTabPanelModule { }

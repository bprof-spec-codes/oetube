import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DevExtremeRoutingModule } from './dev-extreme-routing.module';
import { DevExtremeComponent } from './dev-extreme.component';
import { DxDataGridModule } from 'devextreme-angular';


@NgModule({
  declarations: [
    DevExtremeComponent
  ],
  imports: [
    CommonModule,
    DevExtremeRoutingModule,
    DxDataGridModule
  ]
})
export class DevExtremeModule { }

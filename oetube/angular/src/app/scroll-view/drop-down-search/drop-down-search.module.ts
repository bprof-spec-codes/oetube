import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreationTimeSearchItemDirective, DropDownSearchComponent, NameSearchItemDirective, SearchItemDirective } from './drop-down-search.component';

import {
  DxButtonComponent,
  DxButtonGroupModule,
  DxButtonModule,
  DxDateRangeBoxModule,
  DxDropDownButtonModule,
  DxTextBoxModule,
} from 'devextreme-angular';

@NgModule({
  declarations: [
    SearchItemDirective, NameSearchItemDirective,
    DropDownSearchComponent, CreationTimeSearchItemDirective
  ],
  imports: [
    CommonModule,
    DxTextBoxModule,
    DxDateRangeBoxModule,
    DxDropDownButtonModule,
    DxButtonModule,
    DxButtonGroupModule
  ],
  exports: [
    DropDownSearchComponent,
    SearchItemDirective, NameSearchItemDirective,CreationTimeSearchItemDirective
  ],
})
export class DropDownSearchModule {}

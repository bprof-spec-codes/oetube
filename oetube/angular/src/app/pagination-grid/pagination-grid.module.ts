import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {DxButtonGroupModule, DxDataGridModule, DxValidatorModule } from 'devextreme-angular';
import { DxoDataSourceModule, DxoPagingModule } from 'devextreme-angular/ui/nested';
import { UserPaginationGridComponent } from './user-pagination-grid/user-pagination-grid.component';


@NgModule({
  declarations: [UserPaginationGridComponent],
  imports: [
    CommonModule,
    DxButtonGroupModule,
    DxoPagingModule,
    DxDataGridModule,
    DxoDataSourceModule,
    DxValidatorModule
  ],
  exports:[UserPaginationGridComponent]
})
export class PaginationGridModule{ 
}

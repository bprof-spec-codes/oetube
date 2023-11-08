import { NgModule, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationGridComponent } from './pagination-grid.component';
import {DxButtonGroupModule, DxDataGridModule } from 'devextreme-angular';
import { DxoPagingModule } from 'devextreme-angular/ui/nested';
import { UserPaginationGridComponent } from './user-pagination-grid/user-pagination-grid.component';



@NgModule({
  declarations: [PaginationGridComponent, UserPaginationGridComponent],
  imports: [
    CommonModule,
    DxButtonGroupModule,
    DxoPagingModule,
    DxDataGridModule
  ],
  exports:[PaginationGridComponent,UserPaginationGridComponent]
})
export class PaginationGridModule{ 

}

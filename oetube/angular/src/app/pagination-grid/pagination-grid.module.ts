import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DxButtonGroupModule, DxDataGridModule, DxSelectBoxModule, DxValidatorModule } from 'devextreme-angular';
import { DxoDataSourceModule, DxoPagingModule } from 'devextreme-angular/ui/nested';
import { UserPaginationGridComponent } from './user-pagination-grid/user-pagination-grid.component';
import { GroupPaginationGridComponent } from './group-pagination-grid/group-pagination-grid.component';
import { AccessGroupPaginationGridComponent } from './group-pagination-grid/access-group-pagination-grid.component';
import { MemberPaginationGridComponent } from './user-pagination-grid/member-pagination-grid.component';
import {  PaginationGridComponent } from './pagination-grid.component';
import { CreatorPaginationGridComponent } from './creator-pagination-grid.component';

@NgModule({
  declarations: [
    PaginationGridComponent,
    CreatorPaginationGridComponent,
    UserPaginationGridComponent,
    MemberPaginationGridComponent,
    GroupPaginationGridComponent,
    AccessGroupPaginationGridComponent,
  ],
  imports: [
    CommonModule,
    DxButtonGroupModule,
    DxoPagingModule,
    DxDataGridModule,
    DxoDataSourceModule,
    DxValidatorModule,
    DxSelectBoxModule
  ],
  exports: [
    UserPaginationGridComponent,
    MemberPaginationGridComponent,
    GroupPaginationGridComponent,
    AccessGroupPaginationGridComponent,
  ],
})
export class PaginationGridModule {}

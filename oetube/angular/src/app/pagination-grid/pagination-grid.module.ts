import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DxButtonGroupModule, DxDataGridModule, DxValidatorModule } from 'devextreme-angular';
import { DxoDataSourceModule, DxoPagingModule } from 'devextreme-angular/ui/nested';
import { UserPaginationGridComponent } from './user-pagination-grid/user-pagination-grid.component';
import { PlaylistPaginationGridComponent } from './playlist-pagination-grid/playlist-pagination-grid.component';
import { VideoPaginationGridComponent } from './video-pagination-grid/video-pagination-grid.component';
import { GroupPaginationGridComponent } from './group-pagination-grid/group-pagination-grid.component';
import { AccessGroupPaginationGridComponent } from './group-pagination-grid/access-group-pagination-grid.component';
import { MemberPaginationGridComponent } from './user-pagination-grid/member-pagination-grid.component copy';
import { VideoItemPaginationGridComponent } from './video-pagination-grid/video-item-pagination-grid.component copy';

@NgModule({
  declarations: [
    UserPaginationGridComponent,
    MemberPaginationGridComponent,
    GroupPaginationGridComponent,
    AccessGroupPaginationGridComponent,
    PlaylistPaginationGridComponent,
    VideoPaginationGridComponent,
    VideoItemPaginationGridComponent
  ],
  imports: [
    CommonModule,
    DxButtonGroupModule,
    DxoPagingModule,
    DxDataGridModule,
    DxoDataSourceModule,
    DxValidatorModule,
  ],
  exports: [
    UserPaginationGridComponent,
    MemberPaginationGridComponent,
    GroupPaginationGridComponent,
    AccessGroupPaginationGridComponent,
    VideoPaginationGridComponent,
    PlaylistPaginationGridComponent,
    VideoItemPaginationGridComponent
  ],
})
export class PaginationGridModule {}

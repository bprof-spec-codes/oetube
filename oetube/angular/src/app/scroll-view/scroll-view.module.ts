import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  DxButtonGroupModule,
  DxButtonModule,
  DxCheckBoxModule,
  DxContextMenuModule,
  DxDateBoxModule,
  DxDateRangeBoxModule,
  DxDraggableModule,
  DxDropDownButtonModule,
  DxListModule,
  DxLoadPanelModule,
  DxPopoverModule,
  DxPopupModule,
  DxScrollViewModule,
  DxSelectBoxModule,
  DxSortableModule,
  DxTextBoxModule,
  DxTileViewModule,
  DxToolbarModule,
} from 'devextreme-angular';
import { ScrollViewComponent } from './scroll-view.component';
import { DropDownSearchModule } from './drop-down-search/drop-down-search.module';
import { ScrollViewSelectorPopupComponent } from './scroll-view-selector-popup/scroll-view-selector-popup.component';
import { TemplateRefCollectionModule } from '../template-ref-collection/template-ref-collection.module';
import { DateSearchItemComponent } from './drop-down-search/date-search-item/date-search-item.component';
import { ScrollViewDataSourceComponent } from './scroll-view-data-source/scroll-view-data-source.component';
import { ScrollViewContentsComponent } from './scroll-view-contents/scroll-view-contents.component';
import { GroupDataSourceComponent } from './group/group-data-source/group-data-source.component';
import { GroupSearchComponent } from './group/group-search/group-search.component';
import { GroupContentsComponent } from './group/group-contents/group-contents.component';
import { RouterModule } from '@angular/router';
import { UserSearchComponent } from './user/user-search/user-search.component';
import { UserDataSourceComponent } from './user/user-data-source/user-data-source.component';
import { UserContentsComponent } from './user/user-contents/user-contents.component';
import { VideoSearchComponent } from './video/video-search/video-search.component';
import { VideoContentsComponent } from './video/video-contents/video-contents.component';
import { VideoDataSourceComponent } from './video/video-data-source/video-data-source.component';
import { VideoListItemComponent } from './video/video-contents/video-list-item/video-list-item.component';
import { PlaylistContentsComponent } from './playlist/playlist-contents/playlist-contents.component';
import { PlaylistDataSourceComponent } from './playlist/playlist-data-source/playlist-data-source.component';
import { PlaylistSearchComponent } from './playlist/playlist-search/playlist-search.component';
import { MemberDataSourceComponent } from './user/member-data-source/member-data-source.component';
import { AccessGroupDataSourceComponent } from './group/access-group-data-source/access-group-data-source.component';
import { PlaylistVideoDataSourceComponent } from './video/playlist-video-data-source/playlist-video-data-source.component';
import { UserTileItemComponent } from './user/user-contents/user-tile-item/user-tile-item.component';
import { UserListItemComponent } from './user/user-contents/user-list-item/user-list-item.component';
import { GroupListItemComponent } from './group/group-contents/group-list-item/group-list-item.component';
import { GroupTileItemComponent } from './group/group-contents/group-tile-item/group-tile-item.component';
import { DropDownSearchComponent } from './drop-down-search/drop-down-search.component';
import { VideoTileItemComponent } from './video/video-contents/video-tile-item/video-tile-item.component';
import { AuthModule } from '../auth/auth.module';

@NgModule({
  declarations: [
    ScrollViewDataSourceComponent,
    ScrollViewContentsComponent,

    ScrollViewComponent,
    ScrollViewSelectorPopupComponent,

    AccessGroupDataSourceComponent,
    GroupDataSourceComponent,
    GroupSearchComponent,
    GroupContentsComponent,
    GroupListItemComponent,
    GroupTileItemComponent,
    
    MemberDataSourceComponent,
    UserSearchComponent,
    UserDataSourceComponent,
    UserContentsComponent,
    UserTileItemComponent,
    UserListItemComponent,
    
    VideoSearchComponent,
    VideoContentsComponent,
    VideoDataSourceComponent,
    PlaylistVideoDataSourceComponent,
    VideoListItemComponent,
    VideoTileItemComponent,

    PlaylistDataSourceComponent,
    PlaylistContentsComponent,
    PlaylistSearchComponent,
  ],
  imports: [
    TemplateRefCollectionModule,
    CommonModule,
    DxScrollViewModule,
    DxDraggableModule,
    DxButtonModule,
    DxCheckBoxModule,
    DxToolbarModule,
    DxTextBoxModule,
    DxSelectBoxModule,
    DxLoadPanelModule,
    DxButtonModule,
    DxDateRangeBoxModule,
    DxDropDownButtonModule,
    DxButtonGroupModule,
    DropDownSearchModule,
    DxPopoverModule,
    DxTileViewModule,
    DxSortableModule,
    RouterModule,
    DxPopupModule,
    DxListModule,
    AuthModule
  ],
  exports: [
    DropDownSearchModule,
    ScrollViewComponent,
    ScrollViewSelectorPopupComponent,
    ScrollViewDataSourceComponent,
    ScrollViewContentsComponent,


    AccessGroupDataSourceComponent,
    GroupDataSourceComponent,
    GroupSearchComponent,
    GroupContentsComponent,
    GroupListItemComponent,
    GroupTileItemComponent,
    
    MemberDataSourceComponent,
    UserSearchComponent,
    UserDataSourceComponent,
    UserContentsComponent,
    UserTileItemComponent,
    UserListItemComponent,
    
    VideoSearchComponent,
    VideoContentsComponent,
    VideoDataSourceComponent,
    PlaylistVideoDataSourceComponent,
    VideoListItemComponent,

    PlaylistDataSourceComponent,
    PlaylistContentsComponent,
    PlaylistSearchComponent,
  ],
})
export class ScrollViewModule {}

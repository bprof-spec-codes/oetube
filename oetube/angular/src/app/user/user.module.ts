import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';
import { UserExploreComponent } from './user-explore/user-explore.component';
import { LazyTabPanelModule } from '../lazy-tab-panel/lazy-tab-panel.module';
import { ScrollViewModule } from '../scroll-view/scroll-view.module';
import { UserListItemComponent } from './user-explore/user-list-item/user-list-item.component';
import { UserTileItemComponent } from './user-explore/user-tile-item/user-tile-item.component';
import { DxDateRangeBoxModule, DxTextBoxModule } from 'devextreme-angular';


@NgModule({
  declarations: [
    UserComponent,
    UserExploreComponent,
    UserListItemComponent,
    UserTileItemComponent,
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    LazyTabPanelModule,
    ScrollViewModule,
    DxTextBoxModule,
    DxDateRangeBoxModule
  ],
  exports:[UserComponent,UserExploreComponent,UserListItemComponent,UserTileItemComponent]
})
export class UserModule { }

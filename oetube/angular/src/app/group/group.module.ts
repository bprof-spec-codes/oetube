import { NgModule } from '@angular/core';
import { GroupRoutingModule } from './group-routing.module';
import {
  GroupCreateComponent,
  GroupEditorComponent,
  GroupUpdateComponent,
} from './group-editor/group-editor.component';
import { GroupComponent } from './group.component';
import { GroupExploreComponent } from './group-explore/group-explore.component';
import { UserModule } from '../user/user.module';
import { GroupDetailsComponent } from './group-details/group-details.component';
import { GroupSearchComponent } from './group-explore/group-search/group-search.component';
import { GroupDataSourceComponent } from './group-explore/group-data-source/group-data-source.component';
import { AccessGroupDataSourceComponent } from './group-explore/access-group-data-source/access-group-data-source.component';
import { GroupTileItemComponent } from './group-explore/group-contents/group-tile-item/group-tile-item.component';
import { GroupListItemComponent } from './group-explore/group-contents/group-list-item/group-list-item.component';
import { GroupContentsComponent } from './group-explore/group-contents/group-contents.component';
import { CollectorModule } from '../collector.module';
import { UserScrollViewModule } from '../user/user-scroll-view.module';
import { GroupScrollViewModule } from './group-scroll-view.module';
@NgModule({
  declarations: [
    GroupComponent,
    GroupEditorComponent,
    GroupCreateComponent,
    GroupUpdateComponent,
    GroupExploreComponent,
    GroupDetailsComponent,
    GroupDetailsComponent,
  ],
  imports: [
    CollectorModule,
    GroupRoutingModule,
    UserScrollViewModule,
    GroupScrollViewModule
  ],
})
export class GroupModule {}

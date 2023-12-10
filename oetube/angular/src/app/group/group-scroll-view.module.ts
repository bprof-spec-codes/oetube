import { NgModule } from '@angular/core';
import { CollectorModule } from '../collector.module';
import { AccessGroupDataSourceComponent } from './group-explore/access-group-data-source/access-group-data-source.component';
import { GroupContentsComponent } from './group-explore/group-contents/group-contents.component';
import { GroupListItemComponent } from './group-explore/group-contents/group-list-item/group-list-item.component';
import { GroupTileItemComponent } from './group-explore/group-contents/group-tile-item/group-tile-item.component';
import { GroupDataSourceComponent } from './group-explore/group-data-source/group-data-source.component';
import { GroupSearchComponent } from './group-explore/group-search/group-search.component';


@NgModule({
  declarations: [
    GroupSearchComponent,
    GroupContentsComponent,
    GroupDataSourceComponent,
    AccessGroupDataSourceComponent,
    GroupTileItemComponent,
    GroupListItemComponent
  ],
  imports: [
    CollectorModule,
  ],
  exports:[ 
    GroupSearchComponent,
    GroupContentsComponent,
    GroupDataSourceComponent,
    AccessGroupDataSourceComponent,
    GroupTileItemComponent,
    GroupListItemComponent]
})
export class GroupScrollViewModule { }

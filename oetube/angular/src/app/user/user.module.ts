import { NgModule } from '@angular/core';

import { UserRoutingModule } from './user-routing.module';
import { UserComponent } from './user.component';
import { UserExploreComponent } from './user-explore/user-explore.component';
import { UserSearchComponent } from './user-explore/user-search/user-search.component';
import { UserDataSourceComponent } from './user-explore/user-data-source/user-data-source.component';
import { UserContentsComponent } from './user-explore/user-contents/user-contents.component';
import { MemberDataSourceComponent } from './user-explore/member-data-source/member-data-source.component';
import { CollectorModule } from '../collector.module';
import { UserTileItemComponent } from './user-explore/user-contents/user-tile-item/user-tile-item.component';
import { UserListItemComponent } from './user-explore/user-contents/user-list-item/user-list-item.component';
import { UserDetailsComponent } from './user-details/user-details.component';
import { UserUpdateComponent } from './user-update/user-update.component';
import { VideoModule } from '../video/video.module';
import { PlaylistModule } from '../playlist/playlist.module';
import { GroupModule } from '../group/group.module';
import { UserScrollViewModule } from './user-scroll-view.module';
import { GroupScrollViewModule } from '../group/group-scroll-view.module';
import { VideoScrollViewModule } from '../video/video-scroll-view.module';
import { PlaylistScrollViewModule } from '../playlist/playlist-scroll-view.module';


@NgModule({
  declarations: [
    UserComponent,
    UserExploreComponent,
    UserDetailsComponent,
    UserUpdateComponent,
  ],
  imports: [
    UserRoutingModule,
    UserScrollViewModule,
    GroupScrollViewModule,
    VideoScrollViewModule,
    PlaylistScrollViewModule,
    CollectorModule,
  ],
})
export class UserModule { }

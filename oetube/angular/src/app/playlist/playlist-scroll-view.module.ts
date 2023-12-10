import { NgModule } from '@angular/core';
import { CollectorModule } from '../collector.module';
import { VideoScrollViewModule } from '../video/video-scroll-view.module';
import { PlaylistContentsComponent } from './playlist-explore/playlist-contents/playlist-contents.component';
import { PlaylistListItemComponent } from './playlist-explore/playlist-contents/playlist-list-item/playlist-list-item.component';
import { PlaylistDataSourceComponent } from './playlist-explore/playlist-data-source/playlist-data-source.component';
import { PlaylistSearchComponent } from './playlist-explore/playlist-search/playlist-search.component';


@NgModule({
  declarations: [
    PlaylistSearchComponent,
    PlaylistDataSourceComponent,
    PlaylistContentsComponent,
    PlaylistListItemComponent,
  ],
  imports: [
    CollectorModule,
  ],
  exports:[ 
    PlaylistSearchComponent,
    PlaylistDataSourceComponent,
    PlaylistContentsComponent,
    PlaylistListItemComponent]
})
export class PlaylistScrollViewModule { }

import { NgModule } from '@angular/core';
import { CollectorModule } from '../collector.module';
import { PlaylistListItemComponent } from '../playlist/playlist-explore/playlist-contents/playlist-list-item/playlist-list-item.component';
import { PlaylistVideoDataSourceComponent } from './video-explore/playlist-video-data-source/playlist-video-data-source.component';
import { VideoContentsComponent } from './video-explore/video-contents/video-contents.component';
import { VideoListItemComponent } from './video-explore/video-contents/video-list-item/video-list-item.component';
import { VideoTileItemComponent } from './video-explore/video-contents/video-tile-item/video-tile-item.component';
import { VideoDataSourceComponent } from './video-explore/video-data-source/video-data-source.component';
import { VideoSearchComponent } from './video-explore/video-search/video-search.component';




@NgModule({
  declarations: [
    VideoSearchComponent,
    VideoDataSourceComponent,
    VideoContentsComponent,
    VideoTileItemComponent,
    VideoListItemComponent,
    PlaylistVideoDataSourceComponent
  ],
  imports: [
    CollectorModule,
  ],
  exports:[ 
    VideoSearchComponent,
    VideoDataSourceComponent,
    VideoContentsComponent,
    VideoTileItemComponent,
    VideoListItemComponent,
    PlaylistVideoDataSourceComponent]
})
export class VideoScrollViewModule { }

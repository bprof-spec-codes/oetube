
import { ControlBarComponent } from './video-player/control-bar/control-bar.component';
import { NgModule } from '@angular/core';
import { PlayControlComponent } from './video-player/play-control/play-control.component';
import { SearchBarComponent } from './video-grid/search-bar/search-bar.component';
import { TimeComponent } from './video-player/time/time.component';
import { VideoComponent } from './video.component';
import { VideoGridComponent } from './video-grid/video-grid/video-grid.component';
import { VideoPlayerComponent } from './video-player/video-player.component';
import { VideoRoutingModule } from './video-routing.module';
import { VideoSeekerComponent } from './video-player/video-seeker/video-seeker.component';
import { VideoWrapperComponent } from './video-player/video-wrapper/video-wrapper.component';
import { VolumeControlComponent } from './video-player/volume-control/volume-control.component';
import { SidebarModule } from '../sidebar/sidebar.module';
import { VideoDataSourceComponent } from './video-explore/video-data-source/video-data-source.component';
import { VideoContentsComponent } from './video-explore/video-contents/video-contents.component';
import { VideoSearchComponent } from './video-explore/video-search/video-search.component';
import { VideoTileItemComponent } from './video-explore/video-contents/video-tile-item/video-tile-item.component';
import { VideoListItemComponent } from './video-explore/video-contents/video-list-item/video-list-item.component';
import { CollectorModule } from '../collector.module';
import { GroupModule } from '../group/group.module';
import { PlaylistVideoDataSourceComponent } from './video-explore/playlist-video-data-source/playlist-video-data-source.component';
import { VideoUploadComponent } from './video-upload/video-upload.component';
import { VideoScrollViewModule } from './video-scroll-view.module';
import { GroupScrollViewModule } from '../group/group-scroll-view.module';

@NgModule({
  declarations: [
    VideoComponent,
    VideoPlayerComponent,
    VolumeControlComponent,
    PlayControlComponent,
    VideoSeekerComponent,
    VideoWrapperComponent,
    TimeComponent,
    ControlBarComponent,
    VideoGridComponent,
    SearchBarComponent,
    VideoUploadComponent,
  ],
  imports: [
    VideoRoutingModule,
    CollectorModule,
    GroupScrollViewModule,
    VideoScrollViewModule
  ],
})
export class VideoModule {}



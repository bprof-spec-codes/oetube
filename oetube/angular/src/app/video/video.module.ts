import { CommonModule } from '@angular/common';
import { ControlBarComponent } from './video-player/control-bar/control-bar.component';
import { DxSliderModule } from 'devextreme-angular';
import { NgModule } from '@angular/core';
import { PlayControlComponent } from './video-player/play-control/play-control.component';
import { TimeComponent } from './video-player/time/time.component';
import { VideoComponent } from './video.component';
import { VideoPlayerComponent } from './video-player/video-player.component';
import { VideoRoutingModule } from './video-routing.module';
import { VideoSeekerComponent } from './video-player/video-seeker/video-seeker.component';
import { VideoWrapperComponent } from './video-player/video-wrapper/video-wrapper.component';
import { VolumeControlComponent } from './video-player/volume-control/volume-control.component';
import { VideoListComponent } from './video-player/video-list/video-list.component';

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
    VideoListComponent,
  ],
  imports: [CommonModule, VideoRoutingModule, DxSliderModule],
})
export class VideoModule {}

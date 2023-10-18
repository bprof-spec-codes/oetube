import { DxButtonModule, DxSliderModule, DxTextBoxModule } from 'devextreme-angular';

import { CommonModule } from '@angular/common';
import { ControlBarComponent } from './video-player/control-bar/control-bar.component';
import { NgModule } from '@angular/core';
import { PlayControlComponent } from './video-player/play-control/play-control.component';
import { TimeComponent } from './video-player/time/time.component';
import { VideoComponent } from './video.component';
import { VideoGridComponent } from './video-grid/video-grid/video-grid.component';
import { VideoPlayerComponent } from './video-player/video-player.component';
import { VideoRoutingModule } from './video-routing.module';
import { VideoSeekerComponent } from './video-player/video-seeker/video-seeker.component';
import { VideoWrapperComponent } from './video-player/video-wrapper/video-wrapper.component';
import { VolumeControlComponent } from './video-player/volume-control/volume-control.component';
import { SearchBarComponent } from './video-grid/search-bar/search-bar.component';

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
  ],
  imports: [CommonModule, VideoRoutingModule, DxSliderModule, DxButtonModule, DxTextBoxModule],
})
export class VideoModule {}

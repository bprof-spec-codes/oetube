import { RouterModule, Routes } from '@angular/router';

import { NgModule } from '@angular/core';
import { VideoComponent } from './video.component';
import { VideoPlayerComponent } from './video-player/video-player.component';
import { VideoDetailsComponent } from './video-details/video-details.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: VideoComponent,
  },
  {
    path: ':id',
    component: VideoDetailsComponent,
  },
  {
    path: ':id/:playlist',
    component: VideoDetailsComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VideoRoutingModule {}

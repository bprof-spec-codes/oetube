import { RouterModule, Routes } from '@angular/router';

import { NgModule } from '@angular/core';
import { VideoComponent } from './video.component';
import { VideoPlayerComponent } from './video-player/video-player.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: VideoComponent,
  },
  {
    path: 'watch/:id',
    component: VideoPlayerComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VideoRoutingModule {}

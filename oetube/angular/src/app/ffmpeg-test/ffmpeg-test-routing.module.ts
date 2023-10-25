import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FfmpegTestComponent } from './components/ffmpeg-test/ffmpeg-test.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: FfmpegTestComponent,
  }
 ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FfmpegTestRoutingModule { }

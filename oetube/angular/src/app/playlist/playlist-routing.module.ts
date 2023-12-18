import { RouterModule, Routes } from '@angular/router';

import { NgModule } from '@angular/core';
import { PlaylistComponent } from './playlist.component';
import { PlaylistDetailsComponent } from './playlist-details/playlist-details.component';

const routes: Routes = [
  { path: '', component: PlaylistComponent },
  { path: ':id', component: PlaylistDetailsComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PlaylistRoutingModule {}

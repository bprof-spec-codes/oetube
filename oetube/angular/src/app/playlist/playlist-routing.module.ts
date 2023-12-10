import { RouterModule, Routes } from '@angular/router';

import { NgModule } from '@angular/core';
import { PlaylistComponent } from './playlist.component';
import { PlaylistCreateComponent } from './playlist-create/playlist-create.component';
import { PlaylistViewComponent } from './playlist-view/playlist-view.component';

const routes: Routes = [
  { path: '', component: PlaylistComponent },
  { path: 'create', component: PlaylistCreateComponent },,
  { path: ':id', component: PlaylistViewComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PlaylistRoutingModule {}

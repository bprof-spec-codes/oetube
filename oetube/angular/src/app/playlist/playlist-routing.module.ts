import { RouterModule, Routes } from '@angular/router';

import { NgModule } from '@angular/core';
import { PlaylistComponent } from './playlist.component';
import { PlaylistCreateComponent } from './playlist-create/playlist-create.component';
import { PlaylistYourListsComponent } from './playlist-your-lists/playlist-your-lists.component';

const routes: Routes = [
  { path: '', component: PlaylistComponent },
  { path: 'create', component: PlaylistCreateComponent },
  { path: 'playlists', component: PlaylistYourListsComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PlaylistRoutingModule {}

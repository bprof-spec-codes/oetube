import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlaylistRoutingModule } from './playlist-routing.module';
import { PlaylistComponent } from './playlist.component';
import { PlaylistCreateComponent } from './playlist-create/playlist-create.component';
import { DxTabPanelModule } from 'devextreme-angular';


@NgModule({
  declarations: [
    PlaylistComponent,
    PlaylistCreateComponent
  ],
  imports: [
    CommonModule,
    PlaylistRoutingModule,
    DxTabPanelModule
  ]
})
export class PlaylistModule { }

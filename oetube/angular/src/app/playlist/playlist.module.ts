import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlaylistRoutingModule } from './playlist-routing.module';
import { PlaylistComponent } from './playlist.component';
import { PlaylistCreateComponent } from './playlist-create/playlist-create.component';
import { PlaylistViewComponent } from './playlist-view/playlist-view.component';
import { DxButtonModule, DxDataGridModule, DxTabPanelModule, DxTemplateModule, DxTreeListModule } from 'devextreme-angular';
import { AuthUrlPipe } from '../services/auth-url-pipe/auth-url.pipe';


@NgModule({
  declarations: [
    PlaylistComponent,
    PlaylistCreateComponent,
    PlaylistViewComponent
  ],
  imports: [
    CommonModule,
    PlaylistRoutingModule,
    DxTabPanelModule,
    DxDataGridModule,
    DxTemplateModule,
    DxButtonModule,
    DxTreeListModule,
    AuthUrlPipe
  ]
})
export class PlaylistModule { }

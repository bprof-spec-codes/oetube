import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlaylistRoutingModule } from './playlist-routing.module';
import { PlaylistComponent } from './playlist.component';
import { PlaylistCreateComponent } from './playlist-create/playlist-create.component';
import { PlaylistViewComponent } from './playlist-view/playlist-view.component';
import { DxButtonModule, DxDataGridModule, DxTabPanelModule, DxTemplateModule } from 'devextreme-angular';


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
    DxButtonModule
  ]
})
export class PlaylistModule { }

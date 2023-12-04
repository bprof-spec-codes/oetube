import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlaylistRoutingModule } from './playlist-routing.module';
import { PlaylistComponent } from './playlist.component';
import { PlaylistCreateComponent } from './playlist-create/playlist-create.component';
import { PlaylistYourListsComponent } from './playlist-your-lists/playlist-your-lists.component';
import { DxFileUploaderModule } from 'devextreme-angular';
 
import {
  DxTabPanelModule,
  DxFormModule,
  DxTextAreaModule,
  DxRadioGroupModule
} from 'devextreme-angular';


@NgModule({
  declarations: [
    PlaylistComponent,
    PlaylistCreateComponent,
    PlaylistYourListsComponent
  ],
  imports: [
    CommonModule,
    PlaylistRoutingModule,
    DxTabPanelModule,
    DxFormModule,
    DxTextAreaModule,
    DxRadioGroupModule,
    DxFormModule,
    DxFileUploaderModule,
  ]
})
export class PlaylistModule { }

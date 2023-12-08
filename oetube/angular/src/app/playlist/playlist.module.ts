import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlaylistRoutingModule } from './playlist-routing.module';
import { PlaylistComponent } from './playlist.component';
import { PlaylistCreateComponent } from './playlist-create/playlist-create.component';
import { PlaylistYourListsComponent } from './playlist-your-lists/playlist-your-lists.component';
import { DxFileUploaderModule } from 'devextreme-angular';
import { DxListModule } from 'devextreme-angular';
import { DxButtonModule, DxDataGridModule, DxTemplateModule } from 'devextreme-angular';
import { DxPopupModule } from 'devextreme-angular';
import { ImageUploaderModule } from '../image-uploader/image-uploader.module';
 
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
    DxDataGridModule,
    DxListModule,
    DxTemplateModule,
    DxButtonModule,
    DxPopupModule,
    ImageUploaderModule
  ]
})
export class PlaylistModule { }

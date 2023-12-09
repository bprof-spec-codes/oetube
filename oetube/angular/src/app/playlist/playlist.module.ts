import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlaylistRoutingModule } from './playlist-routing.module';
import { PlaylistComponent } from './playlist.component';
import { PlaylistYourListsComponent } from './playlist-your-lists/playlist-your-lists.component';
import { DxFileUploaderModule } from 'devextreme-angular';
import { DxListModule } from 'devextreme-angular';
import { DxButtonModule, DxDataGridModule, DxTemplateModule } from 'devextreme-angular';
import { DxPopupModule } from 'devextreme-angular';
import { ImageUploaderModule } from '../image-uploader/image-uploader.module';
import {FormsModule,ReactiveFormsModule} from "@angular/forms"
import {
  DxTabPanelModule,
  DxFormModule,
  DxTextAreaModule,
  DxRadioGroupModule
} from 'devextreme-angular';

import { LazyTabPanelModule } from '../lazy-tab-panel/lazy-tab-panel.module';
import { TemplateRefCollectionModule } from '../template-ref-collection/template-ref-collection.module';
import { ScrollViewModule } from '../scroll-view/scroll-view.module';
import { PlaylistCreateComponent } from './playlist-create/playlist-create.component';


@NgModule({
  declarations: [
    PlaylistComponent,
    PlaylistYourListsComponent,
    PlaylistCreateComponent,
  ],
  imports: [
    CommonModule,
    LazyTabPanelModule,
    PlaylistRoutingModule,
    DxTabPanelModule,
    DxFormModule,
    DxTextAreaModule,
    DxRadioGroupModule,
    DxFormModule,
    FormsModule,
    ReactiveFormsModule,
    DxFileUploaderModule,
    DxDataGridModule,
    DxListModule,
    TemplateRefCollectionModule,
    DxTemplateModule,
    DxButtonModule,
    ScrollViewModule,
    DxPopupModule,
    ImageUploaderModule
  ]
})
export class PlaylistModule { }

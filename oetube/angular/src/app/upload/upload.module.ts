import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UploadRoutingModule } from './upload-routing.module';
import { UploadComponent } from './upload.component';
import { FormsModule,ReactiveFormsModule} from '@angular/forms';
import { DxFileUploaderModule, DxTextBoxModule,DxButtonModule,DxSelectBoxModule,DxTextAreaModule,DxProgressBarModule, DxFormModule, DxPopupModule } from 'devextreme-angular';
import { DxRadioGroupModule } from "devextreme-angular";
import { PaginationGridModule } from '../pagination-grid/pagination-grid.module';
import { ScrollViewModule } from '../scroll-view/scroll-view.module';
import { GroupModule } from '../group/group.module';

@NgModule({
  declarations: [
    UploadComponent,
  ],
  imports: [
    CommonModule,
    DxRadioGroupModule,
    UploadRoutingModule,
    FormsModule,
    GroupModule,
    DxFormModule, 
    ReactiveFormsModule,
    DxFileUploaderModule,
    DxTextBoxModule,
    DxTextAreaModule,
    DxButtonModule,
    DxSelectBoxModule,
    ScrollViewModule,
    DxProgressBarModule,
    PaginationGridModule,
    DxPopupModule
  ]
})
export class UploadModule { }

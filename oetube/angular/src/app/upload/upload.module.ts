import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UploadRoutingModule } from './upload-routing.module';
import { UploadComponent } from './upload.component';
import { FormsModule,ReactiveFormsModule} from '@angular/forms';
import { DxFileUploaderModule, DxTextBoxModule,DxButtonModule,DxSelectBoxModule,DxTextAreaModule,DxProgressBarModule, DxFormModule } from 'devextreme-angular';
import { PaginationGridModule } from '../pagination-grid/pagination-grid.module';
import { DxRadioGroupModule } from "devextreme-angular";

@NgModule({
  declarations: [
    UploadComponent,
  ],
  imports: [
    CommonModule,
    DxRadioGroupModule,
    UploadRoutingModule,
    FormsModule,
    DxFormModule,
    ReactiveFormsModule,
    DxFileUploaderModule,
    DxTextBoxModule,
    DxTextAreaModule,
    DxButtonModule,
    DxSelectBoxModule,
    DxProgressBarModule,
    PaginationGridModule
  ]
})
export class UploadModule { }

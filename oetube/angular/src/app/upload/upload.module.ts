import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UploadRoutingModule } from './upload-routing.module';
import { UploadComponent } from './upload.component';
import { FormsModule,ReactiveFormsModule} from '@angular/forms';
import { DxFileUploaderModule, DxTextBoxModule,DxButtonModule,DxSelectBoxModule,DxTextAreaModule,DxProgressBarModule, DxFormModule } from 'devextreme-angular';
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
    ReactiveFormsModule,
    DxFileUploaderModule,
    DxTextBoxModule,
    DxButtonModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxProgressBarModule,
    DxFormModule
  ]
})
export class UploadModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FfmpegTestRoutingModule } from './ffmpeg-test-routing.module';
import { FfmpegTestComponent } from './ffmpeg-test.component';
import { DxButtonModule, DxFileUploaderModule,DxSelectBoxModule,DxTextBoxModule } from 'devextreme-angular';

@NgModule({
  declarations: [
    FfmpegTestComponent
  ],
  imports: [
    CommonModule,
    FfmpegTestRoutingModule,
    DxFileUploaderModule,
    DxTextBoxModule,
    DxButtonModule,
    DxSelectBoxModule
    
  ]
})
export class FfmpegTestModule { }

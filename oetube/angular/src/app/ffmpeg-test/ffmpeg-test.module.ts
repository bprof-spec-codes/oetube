import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FfmpegTestRoutingModule } from './ffmpeg-test-routing.module';
import { FfmpegTestComponent } from './components/ffmpeg-test/ffmpeg-test.component';
import { DxButtonModule, DxFileUploaderModule,DxSelectBoxModule,DxTextBoxModule,DxTextAreaModule, DxProgressBarModule } from 'devextreme-angular';
import { FfmpegTestInfoComponent } from './components/ffmpeg-test-info/ffmpeg-test-info.component';
@NgModule({
  declarations: [
    FfmpegTestComponent,
    FfmpegTestInfoComponent
  ],
  imports: [
    CommonModule,
    FfmpegTestRoutingModule,
    DxFileUploaderModule,
    DxTextBoxModule,
    DxButtonModule,
    DxSelectBoxModule,
    DxTextAreaModule,
    DxProgressBarModule
  ]
})
export class FfmpegTestModule { }

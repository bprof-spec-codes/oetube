import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DxFileUploaderModule } from 'devextreme-angular';
import { ImageUploaderComponent } from './image-uploader.component';



@NgModule({
  declarations: [ImageUploaderComponent],
  imports: [
    DxFileUploaderModule,
    CommonModule,
  ],
  exports:[ImageUploaderComponent]
})
export class ImageUploaderModule { }

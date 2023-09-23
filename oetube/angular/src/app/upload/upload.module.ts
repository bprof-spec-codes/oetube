import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UploadRoutingModule } from './upload-routing.module';
import { UploadComponent } from './upload.component';
import { FFService } from './services/FF.service';


@NgModule({
  declarations: [
    UploadComponent,
  ],
  imports: [
    CommonModule,
    UploadRoutingModule
  ]
})
export class UploadModule { }

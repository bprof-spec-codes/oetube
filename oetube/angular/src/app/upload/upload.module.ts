import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UploadRoutingModule } from './upload-routing.module';
import { UploadComponent } from './upload.component';
import { FFService } from './services/FF.service';
import { FormsModule,ReactiveFormsModule} from '@angular/forms'

@NgModule({
  declarations: [
    UploadComponent,
  ],
  imports: [
    CommonModule,
    UploadRoutingModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class UploadModule { }

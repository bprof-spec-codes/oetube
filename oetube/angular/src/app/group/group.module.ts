
import { GroupComponent } from './group.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupRoutingModule } from './group-routing.module';
import { GroupCreateComponent } from './group-create/group-create.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DxButtonModule, DxFileUploaderModule, DxFormModule, DxTabPanelModule, DxTagBoxModule, DxTextAreaModule, DxTextBoxModule, DxValidatorModule} from 'devextreme-angular';
import { UserModule } from '../user/user.module';
import { PaginationGridModule } from '../pagination-grid/pagination-grid.module';
import { ImageUploaderModule } from '../image-uploader/image-uploader.module';

@NgModule({
  declarations: [
    GroupComponent,
    GroupCreateComponent
  ],
  imports: [
    CommonModule,
    GroupRoutingModule,
    FormsModule,
    DxFormModule,
    ReactiveFormsModule,
    DxTextBoxModule,
    DxValidatorModule,
    DxTextAreaModule,
    DxButtonModule,
    DxTagBoxModule,
    PaginationGridModule,
    DxFileUploaderModule,
    ImageUploaderModule,
    DxTabPanelModule
  ],
})
export class GroupModule {}

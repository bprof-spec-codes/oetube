import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DxButtonModule, DxPopupModule } from 'devextreme-angular';
import { DeleteDialogButtonComponent } from './delete-dialog-button.component';



@NgModule({
  declarations: [DeleteDialogButtonComponent],
  imports: [
    CommonModule,
    DxButtonModule,
    DxPopupModule
  ],
  exports: [DeleteDialogButtonComponent]
})
export class DeleteDialogButtonModule {
  
 }

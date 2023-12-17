import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  DxBarGaugeModule,
  DxButtonModule,
  DxCheckBoxModule,
  DxContextMenuModule,
  DxDataGridModule,
  DxDateRangeBoxModule,
  DxDraggableModule,
  DxDropDownBoxModule,
  DxFileUploaderModule,
  DxFormModule,
  DxLinearGaugeModule,
  DxListModule,
  DxLoadIndicatorModule,
  DxLoadPanelModule,
  DxPopupModule,
  DxProgressBarModule,
  DxRadioGroupModule,
  DxScrollViewModule,
  DxSelectBoxModule,
  DxSliderModule,
  DxTabPanelModule,
  DxTagBoxModule,
  DxTemplateModule,
  DxTextAreaModule,
  DxTextBoxModule,
  DxTileViewComponent,
  DxTileViewModule,
  DxTreeListModule,
  DxValidatorModule,
} from 'devextreme-angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ScrollViewModule } from './scroll-view/scroll-view.module';
import { RouterModule } from '@angular/router';
import { LazyTabPanelModule } from './lazy-tab-panel/lazy-tab-panel.module';
import { ImageUploaderModule } from './image-uploader/image-uploader.module';
import { DxiItemModule, DxoPagerModule, DxoPagingModule } from 'devextreme-angular/ui/nested';
import { TemplateRefCollectionModule } from './template-ref-collection/template-ref-collection.module';
import { SharedModule } from './shared/shared.module';
import { AppPipeModule } from './pipes/auth-url-pipe/app-pipe.module';
import { DoubleClickDirective } from './double-click.directive';

@NgModule({
  imports: [
    CommonModule,

    AppPipeModule,
    ScrollViewModule,
    LazyTabPanelModule,
    ImageUploaderModule,
    TemplateRefCollectionModule,
    ScrollViewModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    DxFormModule,
    DxTileViewModule,
    DxiItemModule,
    DxDropDownBoxModule,
    DxListModule,
    DxTextAreaModule,
    DxRadioGroupModule,
    DxSliderModule,
    DxButtonModule,
    DxTextBoxModule,
    DxLoadIndicatorModule,
    DxDateRangeBoxModule,
    DxPopupModule,
    DxContextMenuModule,
    DxDraggableModule,
    DxValidatorModule,
    DxTemplateModule,
    DxTagBoxModule,
    DxTextAreaModule,
    DxFileUploaderModule,
    DxProgressBarModule,
    DxSelectBoxModule,
    DxoPagerModule,
    DxTreeListModule,
    DxTabPanelModule,
    DxCheckBoxModule,
    DxoPagingModule,
    DxDataGridModule,
    DxScrollViewModule,
  ],
  exports: [
    CommonModule,
    AppPipeModule,
    LazyTabPanelModule,
    ImageUploaderModule,
    TemplateRefCollectionModule,
    ScrollViewModule,
    RouterModule,

    FormsModule,
    ReactiveFormsModule,
    DxFormModule,
    DxLinearGaugeModule,
    
    DxLoadPanelModule,
    DxProgressBarModule,
    DxDropDownBoxModule,
    DxListModule,
    DxTextAreaModule,
    DxRadioGroupModule,
    DxSliderModule,
    DxButtonModule,
    DxTextBoxModule,
    DxLoadIndicatorModule,
    DxDateRangeBoxModule,
    DxPopupModule,
    DxContextMenuModule,
    DxDraggableModule,
    DxValidatorModule,
    DxTemplateModule,
    DxTagBoxModule,
    DxTextAreaModule,
    DxFileUploaderModule,
    DxSelectBoxModule,
    DxoPagerModule,
    DxTreeListModule,
    DxTabPanelModule,
    DxCheckBoxModule,
    DxoPagingModule,
    DxDataGridModule,
    DxScrollViewModule,
    DxTileViewModule,
    DxiItemModule,

  ],
})
export class CollectorModule {}

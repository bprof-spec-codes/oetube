import {  NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DxButtonGroupModule, DxButtonModule, DxCheckBoxModule, DxContextMenuModule, DxDateBoxModule, DxDateRangeBoxModule, DxDraggableModule, DxDropDownButtonModule, DxListModule, DxPopoverModule, DxPopupModule, DxScrollViewModule, DxSelectBoxModule, DxSortableModule, DxTextBoxModule, DxTileViewModule, DxToolbarModule } from 'devextreme-angular';
import { ScrollViewComponent, ScrollViewContentDirective, ScrollViewProviderComponent } from './scroll-view.component';
import { DropDownSearchModule } from './drop-down-search/drop-down-search.module';
import { ScrollViewSelectorPopupComponent } from './scroll-view-selector-popup/scroll-view-selector-popup.component';



@NgModule({
  declarations:[ScrollViewComponent,ScrollViewContentDirective, ScrollViewSelectorPopupComponent
  ],
  imports: [
    CommonModule,
    DxScrollViewModule,
    DxDraggableModule,
    DxButtonModule,
    DxCheckBoxModule,
    DxToolbarModule,
    DxTextBoxModule,
    DxSelectBoxModule,
    DxButtonModule,
    DxDateRangeBoxModule,
    DxDropDownButtonModule,
    DxButtonGroupModule,
    DropDownSearchModule,
    DxPopoverModule,
    DxTileViewModule,
    DxSortableModule,
    DxPopupModule,
    DxListModule
  ], 
  exports:[ScrollViewComponent,ScrollViewContentDirective,DropDownSearchModule,ScrollViewSelectorPopupComponent]
})
export class ScrollViewModule{

 
 }

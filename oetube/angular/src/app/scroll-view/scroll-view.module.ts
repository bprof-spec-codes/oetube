import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  DxButtonGroupModule,
  DxButtonModule,
  DxCheckBoxModule,
  DxContextMenuModule,
  DxDateBoxModule,
  DxDateRangeBoxModule,
  DxDraggableModule,
  DxDropDownButtonModule,
  DxListModule,
  DxLoadPanelModule,
  DxPopoverModule,
  DxPopupModule,
  DxScrollViewModule,
  DxSelectBoxModule,
  DxSortableModule,
  DxTextBoxModule,
  DxTileViewModule,
  DxToolbarModule,
} from 'devextreme-angular';
import { ScrollViewComponent } from './scroll-view.component';
import { DropDownSearchModule } from './drop-down-search/drop-down-search.module';
import { ScrollViewSelectorPopupComponent } from './scroll-view-selector-popup/scroll-view-selector-popup.component';
import { TemplateRefCollectionModule } from '../template-ref-collection/template-ref-collection.module';
import { DateSearchItemComponent } from './drop-down-search/date-search-item/date-search-item.component';
import { ScrollViewDataSourceComponent } from './scroll-view-data-source/scroll-view-data-source.component';
import { ScrollViewContentsComponent } from './scroll-view-contents/scroll-view-contents.component';
import { RouterModule } from '@angular/router';
import { DoubleClickDirective } from '../double-click.directive';

@NgModule({
  declarations: [
    ScrollViewDataSourceComponent,
    ScrollViewContentsComponent,
    ScrollViewComponent,
    ScrollViewSelectorPopupComponent,
  ],
  imports: [
    TemplateRefCollectionModule,
    CommonModule,
    DoubleClickDirective,
    DxScrollViewModule,
    DxDraggableModule,
    DxButtonModule,
    DxCheckBoxModule,
    DxToolbarModule,
    DxTextBoxModule,
    DxSelectBoxModule,
    DxLoadPanelModule,
    DxButtonModule,
    DxDateRangeBoxModule,
    DxDropDownButtonModule,
    DxButtonGroupModule,
    DropDownSearchModule,
    DxPopoverModule,
    DxTileViewModule,
    DxSortableModule,
    RouterModule,
    DxPopupModule,
    DxListModule,
  ],
  exports: [
    DropDownSearchModule,
    ScrollViewComponent,
    ScrollViewSelectorPopupComponent,
    ScrollViewDataSourceComponent,
    ScrollViewContentsComponent,
    TemplateRefCollectionModule
  ],
})
export class ScrollViewModule {}

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DropDownSearchComponent } from './drop-down-search.component';

import {
  DxButtonComponent,
  DxButtonGroupModule,
  DxButtonModule,
  DxDateRangeBoxModule,
  DxDropDownButtonModule,
  DxRangeSelectorModule,
  DxTextBoxModule,
} from 'devextreme-angular';
import { TemplateRefCollectionModule } from 'src/app/template-ref-collection/template-ref-collection.module';
import { TextSearchItemComponent } from './text-search-item/text-search-item.component';
import { DateSearchItemComponent } from './date-search-item/date-search-item.component';
import { DurationSearchItemComponent } from './duration-search-item/duration-search-item.component';

@NgModule({
  declarations: [
   TextSearchItemComponent,
    DropDownSearchComponent, TextSearchItemComponent, DateSearchItemComponent, DurationSearchItemComponent
  ],
  imports: [
    CommonModule,
    DxTextBoxModule,
    DxDateRangeBoxModule,
    DxDropDownButtonModule,
    DxButtonModule,
    DxButtonGroupModule,
    DxRangeSelectorModule,
    TemplateRefCollectionModule
  ],
  exports: [
    DropDownSearchComponent,TextSearchItemComponent,DateSearchItemComponent,DurationSearchItemComponent
  ],
})
export class DropDownSearchModule {}

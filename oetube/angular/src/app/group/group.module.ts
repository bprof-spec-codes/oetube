
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupRoutingModule } from './group-routing.module';
import { GroupCreateComponent, GroupEditorComponent, GroupUpdateComponent } from './group-editor/group-editor.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DxButtonModule,DxCheckBoxModule,DxDataGridModule, DxDateRangeBoxModule, DxDraggableModule, DxFileUploaderModule, DxFormModule, DxListModule, DxLookupModule, DxPopupModule, DxScrollViewModule, DxSelectBoxModule, DxTabPanelComponent, DxTabPanelModule, DxTagBoxModule, DxTextAreaModule, DxTextBoxModule, DxTileViewModule, DxValidatorModule} from 'devextreme-angular';
import { ImageUploaderModule } from '../image-uploader/image-uploader.module';
import { GroupComponent } from './group.component';
import { DxoPagerModule, DxoPagingModule } from 'devextreme-angular/ui/nested';
import { PaginationGridModule } from '../pagination-grid/pagination-grid.module';
import { ScrollViewModule } from '../scroll-view/scroll-view.module';
import { DropDownSearchModule } from '../scroll-view/drop-down-search/drop-down-search.module';
import { GroupExploreComponent } from './group-explore/group-explore.component';
import { LazyTabPanelModule } from '../lazy-tab-panel/lazy-tab-panel.module';
import { UserModule } from '../user/user.module';
import { GroupDetailsComponent } from './group-details/group-details.component';
import { RouterModule,RouterLink } from '@angular/router'
import { TemplateRefCollectionModule } from '../template-ref-collection/template-ref-collection.module';
@NgModule({
  declarations: [
    GroupComponent,
    GroupEditorComponent,
    GroupCreateComponent,
    GroupUpdateComponent,
    GroupExploreComponent,
    GroupDetailsComponent,
  ],
  imports: [
    LazyTabPanelModule,
    DxPopupModule,
    DxDraggableModule,
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
    DropDownSearchModule,
    DxDateRangeBoxModule,
    DxFileUploaderModule,
    ImageUploaderModule,
    DxTabPanelModule,
    DxListModule,
    DxSelectBoxModule,
    DxoPagerModule,
    RouterModule,
    DxCheckBoxModule,
    DxoPagingModule,
    DxDataGridModule, 
    DxScrollViewModule,
    ScrollViewModule,TemplateRefCollectionModule
  ],
  exports:[GroupExploreComponent,GroupDetailsComponent]
})
export class GroupModule {}

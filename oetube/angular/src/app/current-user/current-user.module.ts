import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CurrentUserService } from './services/current-user.service';
import { DxContextMenuModule, DxMenuModule, DxPopoverModule } from 'devextreme-angular';
import { DxiMenuItemModule } from 'devextreme-angular/ui/nested';
import {RouterModule} from '@angular/router'
import { CoreModule, noop } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import {
  NgxValidateCoreModule,
  VALIDATION_ERROR_TEMPLATE,
  VALIDATION_INVALID_CLASSES,
  VALIDATION_TARGET_SELECTOR,
} from '@ngx-validate/core';

@NgModule({
  declarations: [
  ],
  imports: [
    CoreModule,
    ThemeSharedModule,
    NgbCollapseModule,
    NgbDropdownModule,
    NgxValidateCoreModule,
  ],
  exports:[
  ]
})
export class CurrentUserModule { }

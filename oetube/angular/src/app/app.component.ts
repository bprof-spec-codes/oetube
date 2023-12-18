import { RoutesService } from '@abp/ng.core';
import { eThemeBasicComponents, eUserMenuItems } from '@abp/ng.theme.basic';
import { NavItemsService } from '@abp/ng.theme.shared';
import { Component, Directive } from '@angular/core';
import { ValidationStoreService } from './services/validation-store.service';
import { SignalrService } from './services/video/signalr.service';
import { UserMenu, UserMenuService } from '@abp/ng.theme.shared';
import { CurrentUserService } from './current-user/services/current-user.service';
import { Router } from '@angular/router';
import { Abp } from '@proxy/volo';
@Component({
  selector: 'app-root',
  styles: [],
  template: `
    <div>
      <abp-loader-bar></abp-loader-bar>
      <abp-dynamic-layout />
      <app-notify-upload-completed></app-notify-upload-completed>
    </div>
  `,
})
export class AppComponent {
  constructor(
    userMenuService: UserMenuService,
    currentUserService: CurrentUserService,
    routesService:RoutesService,
    router: Router,
    private navItems: NavItemsService,
    private validationStore: ValidationStoreService
  ) {
    routesService.remove(["AbpUiNavigation::Menu:Administration"])
    userMenuService.removeItem(eUserMenuItems.MyAccount);
    userMenuService.addItems([
      {
        textTemplate: { text: 'My account', icon: 'fa fa-cog' },
        order: 1,
        action: () => router.navigate(['/user', currentUserService.getCurrentUser().id]),
      },
    ]);
  }
}

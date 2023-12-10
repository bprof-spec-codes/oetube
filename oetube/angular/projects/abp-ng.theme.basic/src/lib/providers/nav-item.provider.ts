import { APP_INITIALIZER } from '@angular/core';
import { CurrentUserComponent } from '../components/nav-items/current-user.component';
import { NavItemsService } from '@abp/ng.theme.shared';
import { eThemeBasicComponents } from '../enums/components';

export const BASIC_THEME_NAV_ITEM_PROVIDERS = [
  {
    provide: APP_INITIALIZER,
    useFactory: configureNavItems,
    deps: [NavItemsService],
    multi: true,
  },
];

export function configureNavItems(navItems: NavItemsService) {
  return () => {
    navItems.addItems([
      {
        id: eThemeBasicComponents.CurrentUser,
        order: 100,
        component: CurrentUserComponent,
      },
    ]);
  };
}

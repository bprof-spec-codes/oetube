import { RoutesService, eLayoutType } from '@abp/ng.core';
import { CurrentUserComponent, eThemeBasicComponents } from '@abp/ng.theme.basic';
import { eThemeSharedRouteNames } from '@abp/ng.theme.shared';
import { eThemeSharedComponents } from '@abp/ng.theme.shared/extensions';

import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add(
      [
      {
        path: '/',
        name: 'Video',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/playlist',
        name: 'Playlist',
        order: 3,
        layout: eLayoutType.application,
      },
      {
        path:'/group',
        name:'Group',
        order: 4,
        layout:eLayoutType.application
      },
      {
        path:'/user',
        name:'User',
        order:5,
        layout:eLayoutType.application
      }
    ]);
  };
}

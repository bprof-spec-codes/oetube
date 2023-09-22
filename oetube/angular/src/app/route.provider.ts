import { RoutesService, eLayoutType } from '@abp/ng.core';

import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Menu:Home',
        //iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/upload',
        name: 'Upload',
        order: 2,
        layout: eLayoutType.application,
      },
      {
        path: '/playlist/create',
        name: 'Create Playlist',
        order: 3,
        layout: eLayoutType.application,
      },
      {
        path: '/dev-extreme',
        name: 'Dev Extreme',
        order: 4,
        layout: eLayoutType.application,
      },
    ]);
  };
}

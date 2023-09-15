import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'OeTube',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44337/',
    redirectUri: baseUrl,
    clientId: 'OeTube_App',
    responseType: 'code',
    scope: 'offline_access OeTube',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://localhost:44337',
      rootNamespace: 'OeTube',
    },
  },
} as Environment;

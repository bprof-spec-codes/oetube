import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'OeTube',
    logoUrl: '/assets/images/logo/oe_logo.png',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44348/',
    redirectUri: baseUrl,
    clientId: 'OeTube_App',
    responseType: 'code',
    scope: 'offline_access OeTube',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44348',
      rootNamespace: 'OeTube',
    },
  },
} as Environment;

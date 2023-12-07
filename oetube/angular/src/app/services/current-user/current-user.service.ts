import { ConfigStateService } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import {oAuthStorage } from '@abp/ng.oauth'

import { Pipe, PipeTransform } from '@angular/core';




@Injectable({
  providedIn: 'root',
})
export class CurrentUserService {
  getAccessToken(){
    return oAuthStorage.getItem("access_token")
  }
  getAuthorizationHeaderValue():[header:string,value:string]{
    return ["Authorization","bearer "+this.getAccessToken()]
  }
  getCurrentUser(): CurrentUser {
    return this.config.getOne('currentUser');
  }
  constructor(private config: ConfigStateService) {
}

}
export type CurrentUser = {

  isAuthenticated?: boolean;
  id?: string;
  tenantId?: string;
  impersonatorUserId?: string;
  impersonatorTenantId?: string;
  impersonatorUserName?: string;
  impersonatorTenantName?: string;
  userName?: string;
  name?: string;
  surName?: string;
  email?: string;
  emailVerified?: boolean;
  phoneNumber?: string;
  phoneNumberVerified?: boolean;
  roles?: string[];
};


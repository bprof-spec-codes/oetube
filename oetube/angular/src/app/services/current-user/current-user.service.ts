import { BehaviorSubject, Observable } from 'rxjs';
import {ConfigStateService} from '@abp/ng.core'
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CurrentUserService{

    get():{
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
    }
    {
        return this.config.getOne("currentUser")
    }
    constructor(private config:ConfigStateService){
    }
}
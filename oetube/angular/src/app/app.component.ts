import { RoutesService } from '@abp/ng.core';
import { CurrentUserComponent, eThemeBasicComponents } from '@abp/ng.theme.basic';
import { NavItemsService } from '@abp/ng.theme.shared';
import { Component,Directive } from '@angular/core';
import { CurrentUserService } from './auth/current-user/current-user.service';
import { ValidationStoreService } from './services/validation-store.service';
import { SignalrService } from './services/video/signalr.service';

@Component({
  selector: 'app-root',
  styles:[],
  template: `
  <div>
      <abp-loader-bar></abp-loader-bar>
      <abp-dynamic-layout/>
      <app-notify-upload-completed></app-notify-upload-completed>
</div>
  `,
})
export class AppComponent {


constructor(private navItems:NavItemsService,private validationStore:ValidationStoreService){
}

}

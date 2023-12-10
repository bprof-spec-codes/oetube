import { RoutesService } from '@abp/ng.core';
import { CurrentUserComponent, eThemeBasicComponents } from '@abp/ng.theme.basic';
import { NavItemsService } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  styles:[],
  template: `
  <div>
      <abp-loader-bar></abp-loader-bar>
      <abp-dynamic-layout/>
</div>
  `,
})
export class AppComponent {
constructor(private navItems:NavItemsService){
}

}

import { RoutesService } from '@abp/ng.core';
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


}

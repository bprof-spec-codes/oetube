import { Component } from '@angular/core';
import { LazyTabItem } from '../lazy-tab-panel/lazy-tab-panel.component';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent {
  inputItems:LazyTabItem[]=[
    {key:"explore",title:"Explore",isLoaded:true,visible:true,onlyCreator:false,authRequired:false}
  ]
}

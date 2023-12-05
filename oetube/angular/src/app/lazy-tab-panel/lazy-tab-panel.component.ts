import { Component,Directive,ContentChildren,QueryList,Input, AfterContentInit,TemplateRef } from '@angular/core';
import { DxTabPanelComponent } from 'devextreme-angular';
import { CurrentUserService } from '../services/current-user/current-user.service';
@Directive({
  selector: "[appLazyTabItem]"
})
export class LazyTabItemDirective{
  private _appLazyTabItem:any[]
  @Input() set appLazyTabItem(value:any[]){
    this._appLazyTabItem=value
  }
  private _appLazyTabItemTitle:string=""
  @Input() set appLazyTabItemTitle(value:string){
    this._appLazyTabItemTitle=value
  }
  get title():string{
    return this._appLazyTabItemTitle
  }
  private _appLazyTabItemAuth:boolean=false
@Input() set appLazyTabItemAuth(value:boolean){
  this._appLazyTabItemAuth=value
}
get auth():boolean{
  return this._appLazyTabItemAuth
}
  isLoaded:boolean=false
  
  constructor(public tabItemTemplate:TemplateRef<any>){
  }

}


@Component({
  selector: 'app-lazy-tab-panel',
  templateUrl: './lazy-tab-panel.component.html',
  styleUrls: ['./lazy-tab-panel.component.scss']
})
export class LazyTabPanelComponent implements AfterContentInit {
  @ContentChildren(LazyTabItemDirective) tabItemsQuery:QueryList<LazyTabItemDirective>

  selectedItem:LazyTabItemDirective
  tabItems:LazyTabItemDirective[]=[]
  constructor(protected currentUserService: CurrentUserService){
  }
  ngAfterContentInit(): void {
      const currentUser=this.currentUserService.get()
      this.tabItems=this.tabItemsQuery.filter(i=>!i.auth||currentUser.isAuthenticated)
      if(this.tabItems.length>0){
        this.tabItems[0].isLoaded=true
        this.selectedItem=this.tabItems[0]
      }
  }
  onSelectedItemChange(item:LazyTabItemDirective){
    if(!item.isLoaded){
      item.isLoaded=true
    }
  }
}


import { Component,Directive,ContentChildren,QueryList,Input, AfterContentInit,TemplateRef,EventEmitter,Output } from '@angular/core';
import { DxTabPanelComponent } from 'devextreme-angular';
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
  private _appLazyTabItemAuthRequired:boolean=false
@Input() set appLazyTabItemAuth(value:boolean){
  if(!this._appLazyTabItemOnlyCreator){
    this._appLazyTabItemAuthRequired=value
  }
}
get authRequired():boolean{
  return this._appLazyTabItemAuthRequired
}

private _appLazyTabItemOnlyCreator:boolean=false
@Input() set appLazyTabItemOnlyCreator(value:boolean){
  this._appLazyTabItemOnlyCreator=value
  if(value){
    this._appLazyTabItemAuthRequired=true
  }
}
get onlyCreator():boolean{
  return this._appLazyTabItemOnlyCreator
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
  @Input() creatorId?:string
  @Input() currentUserId?:string

  @Input() selectedIndex:number
  @Output() selectedIndexChange:EventEmitter<number>=new EventEmitter<number>()
  tabItems:LazyTabItemDirective[]=[]

 
  itemFilter(directive:LazyTabItemDirective){
     const authResult=(!directive.authRequired|| this.currentUserId)
      if(!authResult){
        return false
      }
      return this.creatorId==null||!directive.onlyCreator||this.currentUserId==this.creatorId
  }
  
  ngAfterContentInit(): void {
      this.tabItems=this.tabItemsQuery.filter(i=>this.itemFilter(i))
      if(this.tabItems.length>0){
        this.tabItems[0].isLoaded=true
        this.selectedIndex=0
      }
  }
  onSelectedIndexChange(e:number){

    if(!this.tabItems[e].isLoaded){
      this.tabItems[e].isLoaded=true
    }
    this.selectedIndexChange.emit(e)

  }
}


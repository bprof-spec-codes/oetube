import { Component,ChangeDetectorRef,AfterViewInit,Input,OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { DxiItemComponent } from 'devextreme-angular/ui/nested';
import { TemplateRefCollectionComponent } from 'src/app/template-ref-collection/template-ref-collection.component';
import { CurrentUser, CurrentUserService } from '../current-user/services/current-user.service';

export type LazyTabItem={
  key:string
  title:string
  authRequired:boolean
  onlyCreator:boolean
  role?:string
  isLoaded:boolean
  visible:boolean
}

@Component({
  selector: 'app-lazy-tab-panel',
  templateUrl: './lazy-tab-panel.component.html',
  styleUrls: ['./lazy-tab-panel.component.scss']
})
export class LazyTabPanelComponent extends TemplateRefCollectionComponent<LazyTabItem> implements AfterViewInit, OnChanges{
  private currentUser:CurrentUser
  private _creatorId:string
  @Input() set  creatorId(v:string){
    if(v!=this.creatorId){
      this._creatorId=v;
      this.filterItems()
    }
  }
  get creatorId(){
    return this._creatorId
  }
  @Output() creatorIdChange:EventEmitter<string>=new EventEmitter<string>()
  constructor(private changeDetector:ChangeDetectorRef,public currentUserService:CurrentUserService){
    super()
    this.currentUser=currentUserService.getCurrentUser()
  }
  filteredItems=[]


  filterItems(){
    this.items.forEach((v,i)=>{

      if((v.authRequired||v.onlyCreator||v.role)&&!this.currentUser.isAuthenticated)
      {
        v.visible=false
      }
      else if(v.role!=undefined&&!this.currentUser.roles.includes(v.role)) {
        v.visible=false
      }
      else{
        v.visible=this.creatorId==null||!v.onlyCreator||this.currentUser.id==this.creatorId
      }
    })
    
 }
 ngOnChanges(changes: SimpleChanges): void {
  const currentUser=this.currentUserService.getCurrentUser()
  if(this.currentUser?.id!=this.currentUser?.id){
    this.currentUser=currentUser
    this.filterItems()
}
 }
  ngAfterViewInit(): void {
      super.ngAfterViewInit()
      this.filterItems()
      this.changeDetector.detectChanges()
    }
  onSelectionChange(e){
    if(this.selectedItem){
      if(!this.selectedItem.isLoaded){
        this.selectedItem.isLoaded=true
      }
    }
  }
}

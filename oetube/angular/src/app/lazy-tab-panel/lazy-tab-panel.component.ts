import { Component,ChangeDetectorRef,AfterViewInit,Input,OnChanges, SimpleChanges } from '@angular/core';
import { DxiItemComponent } from 'devextreme-angular/ui/nested';
import { CurrentUser, CurrentUserService } from 'src/app/auth/current-user/current-user.service';
import { TemplateRefCollectionComponent } from 'src/app/template-ref-collection/template-ref-collection.component';

export type LazyTabItem={
  key:string
  title:string
  authRequired:boolean
  onlyCreator:boolean
  isLoaded:boolean
  visible:boolean
}

@Component({
  selector: 'app-lazy-tab-panel',
  templateUrl: './lazy-tab-panel.component.html',
  styleUrls: ['./lazy-tab-panel.component.scss']
})
export class LazyTabPanelComponent extends TemplateRefCollectionComponent<LazyTabItem> implements AfterViewInit, OnChanges{
  
  @Input() creatorId:string
  constructor(private changeDetector:ChangeDetectorRef,public currentUserService:CurrentUserService){
    super()
  }
  filteredItems=[]


  filterItems(){
    const currentUserId=this.currentUserService.getCurrentUser().id
    
    this.items.forEach((v,i)=>{
      debugger
      const authResult=(!v.authRequired || currentUserId!=undefined)
      if(!authResult){
        v.visible=false
      }
      else{
        v.visible=this.creatorId==null||!v.onlyCreator||currentUserId==this.creatorId
      }
    })
    
 }
 ngOnChanges(changes: SimpleChanges): void {
  console.log("change")
  this.filterItems()
 }
  ngAfterViewInit(): void {
      super.ngAfterViewInit()
      this.filterItems()
      this.changeDetector.detectChanges()
    }
  onSelectionChange(e){
    console.log(e)
    if(this.selectedItem){
      if(!this.selectedItem.isLoaded){
        this.selectedItem.isLoaded=true
      }
    }
  }
}

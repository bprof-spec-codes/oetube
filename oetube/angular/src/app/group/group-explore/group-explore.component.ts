import { Component,Input,forwardRef,ViewChild, ChangeDetectorRef, AfterContentInit, AfterViewInit } from '@angular/core';
import { GroupService } from '@proxy/application';
import { PaginationDto } from '@proxy/application/dtos';
import { GroupListItemDto, GroupQueryDto } from '@proxy/application/dtos/groups';
import { Observable } from 'rxjs';
import { ActionButton, ScrollViewComponent, ScrollViewProviderComponent } from 'src/app/scroll-view/scroll-view.component';


@Component({
  selector: 'app-group-explore',
  templateUrl: './group-explore.component.html',
  styleUrls: ['./group-explore.component.scss'],
  providers:[{
    provide:ScrollViewProviderComponent,useExisting:forwardRef(()=>GroupExploreComponent)
  }]
})
export class GroupExploreComponent extends ScrollViewProviderComponent<GroupListItemDto>  implements AfterViewInit{
  @ViewChild(ScrollViewComponent) provider:ScrollViewComponent<GroupListItemDto>
  get scrollView(): ScrollViewComponent<GroupListItemDto> {
    return this.provider
  }
  constructor(public groupService:GroupService){
    super()
  }
  initThis(){
    this.getList=(args)=>this.groupService.getList(args)
    this.scrollView.initScrollView()
  }
}

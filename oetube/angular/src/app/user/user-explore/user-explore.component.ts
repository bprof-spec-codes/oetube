import { Component, Input,forwardRef,AfterViewInit,ViewChild, ChangeDetectorRef } from '@angular/core';
import { OeTubeUserService } from '@proxy/application';
import { UserListItemDto } from '@proxy/application/dtos/oe-tube-users';
import { ActionButton, ScrollViewComponent, ScrollViewOptions, ScrollViewProviderComponent } from 'src/app/scroll-view/scroll-view.component';

@Component({
  selector: 'app-user-explore',
  templateUrl: './user-explore.component.html',
  styleUrls: ['./user-explore.component.scss'],
  providers:[{provide:ScrollViewProviderComponent,useExisting:forwardRef(()=>UserExploreComponent)}]
})
export class UserExploreComponent extends ScrollViewProviderComponent<UserListItemDto> {
  @ViewChild(ScrollViewComponent) provider:ScrollViewComponent<UserListItemDto>
  get scrollView(): ScrollViewComponent<UserListItemDto> {
     return this.provider 
  }
  constructor(public userService:OeTubeUserService){
    super()
  }
  initThis(): void {
    this.getList=(args)=>this.userService.getList(args)
  }
}

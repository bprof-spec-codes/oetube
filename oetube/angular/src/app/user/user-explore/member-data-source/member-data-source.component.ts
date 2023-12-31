import { Component, forwardRef, Input, ViewChild } from '@angular/core';
import { GroupService, OeTubeUserService } from '@proxy/application';
import { UserListItemDto, UserQueryDto } from '@proxy/application/dtos/oe-tube-users';
import { UserDataSourceComponent } from '../user-data-source/user-data-source.component';
import { Observable } from 'rxjs';
import { PaginationDto } from '@proxy/application/dtos';
import { DataSourceProviderDirective, ScrollViewDataSourceComponent } from 'src/app/scroll-view/scroll-view-data-source/scroll-view-data-source.component';
import { CurrentUserService } from 'src/app/current-user/services/current-user.service';





@Component({
  selector: 'app-member-data-source',
  templateUrl: './member-data-source.component.html',
  styleUrls: ['./member-data-source.component.scss'],
  providers:[{provide:DataSourceProviderDirective,useExisting:forwardRef(()=>MemberDataSourceComponent)}]
})
export class MemberDataSourceComponent extends DataSourceProviderDirective<UserListItemDto> {

  @ViewChild(ScrollViewDataSourceComponent) _scrollViewDataSourceComponent:ScrollViewDataSourceComponent<UserListItemDto>
  get scrollViewDataSourceComponent(): ScrollViewDataSourceComponent<UserListItemDto>  {
    return this._scrollViewDataSourceComponent
  }

  @Input() groupId:string

  filteredKeys:string[]
  constructor(public userService:OeTubeUserService, public groupService:GroupService,public currentUserService:CurrentUserService){
    super()
    this.filteredKeys=[currentUserService.getCurrentUser().id]
  }
  getMethod=(args)=>this.userService.getList(args)
  getInitialSelectionMethod=(id,args)=>this.groupService.getExplicitMembersByIdAndInput(id,args)
}

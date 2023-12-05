import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { GroupService, OeTubeUserService } from '@proxy/application';
import { PaginationDto } from '@proxy/application/dtos';
import { UserListItemDto, UserQueryDto } from '@proxy/application/dtos/oe-tube-users';
import { LoadOptions } from 'devextreme/data';
import DataSource from 'devextreme/data/data_source';
import { Observable } from 'rxjs';
import { PaginationGridComponent } from '../pagination-grid.component';
import { UserPaginationGridComponent } from './user-pagination-grid.component';
import {ConfigStateService} from '@abp/ng.core'
import { ActivatedRoute, Router } from '@angular/router';




@Component({
  selector: 'app-member-pagination-grid',
  templateUrl: '../pagination-grid.component.html',
  styleUrls:['../pagination-grid.component.scss']
})

export class MemberPaginationGridComponent extends
  UserPaginationGridComponent{
  @Input() groupId:string

  constructor(protected userService:OeTubeUserService,protected groupService: GroupService) {
    super(userService)
  }
getList(query: UserQueryDto): Observable<PaginationDto<UserListItemDto>> {
    return this.groupService.getGroupMembers(this.groupId,query)
}

}

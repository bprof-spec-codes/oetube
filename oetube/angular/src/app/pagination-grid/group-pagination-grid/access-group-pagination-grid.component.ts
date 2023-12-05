import { Component,Input } from '@angular/core';
import { GroupService, VideoService } from '@proxy/application';
import { PaginationDto } from '@proxy/application/dtos';
import { GroupListItemDto, GroupQueryDto } from '@proxy/application/dtos/groups';
import { LoadOptions } from 'devextreme/data';
import { Observable } from 'rxjs';
import { PaginationGridComponent } from '../pagination-grid.component';
import { GroupPaginationGridComponent } from './group-pagination-grid.component';
import {ConfigStateService} from '@abp/ng.core'
import { CurrentUserService } from 'src/app/services/current-user/current-user.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-access-group-pagination-grid',
  templateUrl: '../pagination-grid.component.html',
  styleUrls: ['../pagination-grid.component.scss']
})
export class AccessGroupPaginationGridComponent extends GroupPaginationGridComponent{
  videoId:string
  constructor(protected activatedRoute:ActivatedRoute,protected router:Router, protected currentUserService: CurrentUserService, protected groupService: GroupService,protected videoService:VideoService) {
      super(currentUserService,groupService)
  }

  getList(query: GroupQueryDto): Observable<PaginationDto<GroupListItemDto>> {
      return this.videoService.getAccessGroups(this.videoId,query)
  }  
}
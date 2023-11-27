import { Component,Input } from '@angular/core';
import { GroupService, VideoService } from '@proxy/application';
import { PaginationDto } from '@proxy/application/dtos';
import { GroupListItemDto, GroupQueryDto } from '@proxy/application/dtos/groups';
import { LoadOptions } from 'devextreme/data';
import { Observable } from 'rxjs';
import { PaginationGridComponent } from '../pagination-grid.component';
import { GroupPaginationGridComponent } from './group-pagination-grid.component';

@Component({
  selector: 'app-access-group-pagination-grid',
  templateUrl: './group-pagination-grid.component.html',
  styleUrls: ['./group-pagination-grid.component.scss']
})
export class AccessGroupPaginationGridComponent extends GroupPaginationGridComponent{

  @Input() videoId:string
  constructor(public groupService:GroupService,public videoService:VideoService){
    super(groupService)
  }
  
  getList(): Observable<PaginationDto<GroupListItemDto>> {
      return this.videoService.getAccessGroups(this.videoId,this.queryArgs)
  }
}
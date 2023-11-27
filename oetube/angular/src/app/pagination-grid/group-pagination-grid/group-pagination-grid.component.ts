import { Component,Input } from '@angular/core';
import { GroupService, VideoService } from '@proxy/application';
import { PaginationDto } from '@proxy/application/dtos';
import { CreateUpdateGroupDto, GroupDto, GroupListItemDto, GroupQueryDto } from '@proxy/application/dtos/groups';
import { LoadOptions } from 'devextreme/data';
import { Observable } from 'rxjs';
import { PaginationGridComponent } from '../pagination-grid.component';

@Component({
  selector: 'app-group-pagination-grid',
  templateUrl: './group-pagination-grid.component.html',
  styleUrls: ['./group-pagination-grid.component.scss']
})
export class GroupPaginationGridComponent 
extends PaginationGridComponent<GroupQueryDto,GroupDto,GroupListItemDto,CreateUpdateGroupDto>
{
  @Input() showName:boolean=true
  @Input() showId:boolean=true
  @Input() showThumbnailImage:boolean=true
  @Input() showCreationTime:boolean=true
  @Input() showTotalMembersCount:boolean=true
  @Input() showIsMember:boolean=true
  @Input() showCreator:boolean=true
  @Input() creatorIdFilter?:string

  constructor(public groupService:GroupService){
    super()
  }

getList(): Observable<PaginationDto<GroupListItemDto>> {
    return this.groupService.getList(this.queryArgs)
}

  handleFilter(options: LoadOptions): void {
  
    this.queryArgs.name = this.findFilterValue(options.filter, "contains", "name")
    this.queryArgs.creationTimeMin = this.findFilterValue(options.filter, ">=", "creationTime")
    this.queryArgs.creationTimeMax = this.findFilterValue(options.filter, "<", "creationTime")
    this.queryArgs.creatorId=this.creatorIdFilter
  }
}


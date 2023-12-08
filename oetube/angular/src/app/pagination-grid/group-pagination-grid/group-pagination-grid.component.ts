import { Component, Input } from '@angular/core';
import { GroupService, VideoService } from '@proxy/application';
import { PaginationDto, QueryDto } from '@proxy/application/dtos';
import {
  GroupListItemDto,
  GroupQueryDto,
} from '@proxy/application/dtos/groups';
import { Observable } from 'rxjs';
import { CreatorPaginationGridComponent as CreatorPaginationGridComponent } from '../creator-pagination-grid.component';
import { Builder, Cloner } from 'src/app/base-types/builder';
import { Column } from 'devextreme/ui/data_grid'
import { ThumbnailColumnBuilder } from '../columns';
import { CurrentUserService } from 'src/app/auth/current-user/current-user.service';

@Component({
  selector: 'app-group-pagination-grid',
  templateUrl: '../pagination-grid.component.html',
  styleUrls: ['../pagination-grid.component.scss'],
})
export class GroupPaginationGridComponent extends CreatorPaginationGridComponent<GroupListItemDto> {


  @Input() thumbnail:Column=new ThumbnailColumnBuilder().build()
  @Input() totalMembers:Column=new TotalMembersColumnBuilder().build()


  constructor(protected currentUserService: CurrentUserService, protected groupService: GroupService) {
    super(currentUserService)
  }
  buildColumns(): Column<any, any>[] {
      return [this.thumbnail,this.id,this.name,this.totalMembers,this.creationTime,this.creator]
  }

  getList(query: GroupQueryDto): Observable<PaginationDto<GroupListItemDto>> {
    return this.groupService.getList(query);
  }
}

export class TotalMembersColumnBuilder extends Builder<Column>{
  constructor(defaultAssigner?:Cloner)
  {
    super({
      visible: true,
      dataField: 'totalMembersCount',
      dataType: 'number' ,
      caption: 'Members',
      allowFiltering:false,
      allowSorting:false
  },defaultAssigner)
  }
}

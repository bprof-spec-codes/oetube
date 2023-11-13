import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { OeTubeUserService } from '@proxy/application';
import { UserListItemDto, UserQueryDto } from '@proxy/application/dtos/oe-tube-users';
import { LoadOptions } from 'devextreme/data';
import DataSource from 'devextreme/data/data_source';
import { PaginationGridComponent } from '../pagination-grid.component';




@Component({
  selector: 'app-user-pagination-grid',
  templateUrl: './user-pagination-grid.component.html',
  styleUrls: ['./user-pagination-grid.component.scss']
})

export class UserPaginationGridComponent extends
  PaginationGridComponent<OeTubeUserService, UserQueryDto, UserListItemDto> implements OnInit, OnDestroy {

@Input() showName:boolean=true
@Input() showId:boolean=true
@Input() showThumbnailImage:boolean=true
@Input() showCreationTime:boolean=true
@Input() showEmailDomain:boolean=true


  dataSource: DataSource
  

  constructor(userService: OeTubeUserService) {
    super()
    this.listProvider = userService
  }

  handleFilter(options: LoadOptions): void {
    this.queryArgs.name = this.findFilterValue(options.filter, "contains", "name")
    this.queryArgs.emailDomain = this.findFilterValue(options.filter, "contains", "emailDomain")
    this.queryArgs.creationTimeMin = this.findFilterValue(options.filter, ">=", "creationTime")
    this.queryArgs.creationTimeMax = this.findFilterValue(options.filter, "<", "creationTime")
  }
}

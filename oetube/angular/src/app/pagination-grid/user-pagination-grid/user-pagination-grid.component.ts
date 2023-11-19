import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { OeTubeUserService } from '@proxy/application';
import { PaginationDto } from '@proxy/application/dtos';
import { UpdateUserDto, UserDto, UserListItemDto, UserQueryDto } from '@proxy/application/dtos/oe-tube-users';
import { LoadOptions } from 'devextreme/data';
import DataSource from 'devextreme/data/data_source';
import { Observable } from 'rxjs';
import { PaginationGridComponent } from '../pagination-grid.component';




@Component({
  selector: 'app-user-pagination-grid',
  templateUrl: './user-pagination-grid.component.html',
  styleUrls: ['./user-pagination-grid.component.scss']
})

export class UserPaginationGridComponent extends
  PaginationGridComponent<UserQueryDto,UserDto,UserListItemDto,UpdateUserDto>  {

@Input() showName:boolean=true
@Input() showThumbnailImage:boolean=true
@Input() showCreationTime:boolean=true
@Input() showEmailDomain:boolean=true


  constructor(public userService: OeTubeUserService) {
    super()
  }
getList(): Observable<PaginationDto<UserListItemDto>> {
    return this.userService.getList(this.queryArgs)
}
  handleFilter(options: LoadOptions): void {
    this.queryArgs.name = this.findFilterValue(options.filter, "contains", "name")
    this.queryArgs.emailDomain = this.findFilterValue(options.filter, "contains", "emailDomain")
    this.queryArgs.creationTimeMin = this.findFilterValue(options.filter, ">=", "creationTime")
    this.queryArgs.creationTimeMax = this.findFilterValue(options.filter, "<", "creationTime")
  }
}

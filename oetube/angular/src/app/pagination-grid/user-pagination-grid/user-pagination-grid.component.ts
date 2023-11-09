import { Component, OnInit,OnDestroy, } from '@angular/core';
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
PaginationGridComponent<OeTubeUserService,UserQueryDto,UserListItemDto> implements OnInit, OnDestroy{
dataSource:DataSource

  constructor(userService:OeTubeUserService){
    super()
    this.listProvider=userService
    this.listArgs={itemPerPage:10,page:0}
  }

beforeLoad(options: LoadOptions<any>): void {
 
}

}
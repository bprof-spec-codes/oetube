import { Component } from '@angular/core';
import { OeTubeUserService } from '@proxy/application';
import { UserListItemDto, UserQueryDto } from '@proxy/application/dtos/oe-tube-users';
import { BeforeLoadArgs } from '../pagination-grid.component';

@Component({
  selector: 'app-user-pagination-grid',
  templateUrl: './user-pagination-grid.component.html',
  styleUrls: ['./user-pagination-grid.component.scss']
})
export class UserPaginationGridComponent {

  inputArgs:UserQueryDto

  constructor(public userService:OeTubeUserService){
  }

  onBeforeLoad(e:BeforeLoadArgs<UserListItemDto>){
  }
}

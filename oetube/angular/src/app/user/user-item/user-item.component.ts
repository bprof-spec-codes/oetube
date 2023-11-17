import { Component, Input } from '@angular/core';
import { UserListItemDto } from '@proxy/application/dtos/oe-tube-users';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.scss']
})


export class UserItemComponent {
  @Input() user:UserListItemDto
}

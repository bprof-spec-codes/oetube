import { Component,Input } from '@angular/core';
import { UserListItemDto } from '@proxy/application/dtos/oe-tube-users';

@Component({
  selector: 'app-user-list-item',
  templateUrl: './user-list-item.component.html',
  styleUrls: ['./user-list-item.component.scss']
})
export class UserListItemComponent {
  @Input() item:UserListItemDto

}

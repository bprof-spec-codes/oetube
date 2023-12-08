import { Component,Input } from '@angular/core';
import { UserListItemDto } from '@proxy/application/dtos/oe-tube-users';

@Component({
  selector: 'app-user-tile-item',
  templateUrl: './user-tile-item.component.html',
  styleUrls: ['./user-tile-item.component.scss']
})
export class UserTileItemComponent {
  @Input() item:UserListItemDto

}

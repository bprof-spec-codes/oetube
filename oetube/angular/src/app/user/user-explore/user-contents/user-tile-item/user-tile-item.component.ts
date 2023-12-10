import { Component,Input } from '@angular/core';
import { UserListItemDto } from '@proxy/application/dtos/oe-tube-users';
import{Router} from '@angular/router'
@Component({
  selector: 'app-user-tile-item',
  templateUrl: './user-tile-item.component.html',
  styleUrls: ['./user-tile-item.component.scss']
})
export class UserTileItemComponent {
  @Input() item:UserListItemDto
  @Input() contextNavigation=false
  constructor(private router:Router){
  }
  contextItems=[{text:""}]

}

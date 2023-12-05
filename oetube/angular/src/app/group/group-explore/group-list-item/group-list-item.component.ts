import { Component,Input } from '@angular/core';
import { GroupListItemDto } from '@proxy/application/dtos/groups';

@Component({
  selector: 'app-group-list-item',
  templateUrl: './group-list-item.component.html',
  styleUrls: ['./group-list-item.component.scss']
})
export class GroupListItemComponent {
  @Input() item:GroupListItemDto

}

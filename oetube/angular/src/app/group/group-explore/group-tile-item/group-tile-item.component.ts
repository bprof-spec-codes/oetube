import { Component,Input } from '@angular/core';
import { GroupListItemDto } from '@proxy/application/dtos/groups';

@Component({
  selector: 'app-group-tile-item',
  templateUrl: './group-tile-item.component.html',
  styleUrls: ['./group-tile-item.component.scss']
})
export class GroupTileItemComponent {
  @Input() item:GroupListItemDto
}

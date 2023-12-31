import { Component,Input } from '@angular/core';
import { VideoListItemDto } from '@proxy/application/dtos/videos';
@Component({
  selector: 'app-video-tile-item',
  templateUrl: './video-tile-item.component.html',
  styleUrls: ['./video-tile-item.component.scss']
})
export class VideoTileItemComponent {

  @Input() item:VideoListItemDto
  @Input() contextNavigation:boolean=true
  @Input() contextItems=[""]
  @Input() getRoute:Function
}

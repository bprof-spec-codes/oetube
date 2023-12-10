import { Component, Input } from '@angular/core';
import { VideoListItemDto } from '@proxy/application/dtos/videos';

@Component({
  selector: 'app-video-list-item',
  templateUrl: './video-list-item.component.html',
  styleUrls: ['./video-list-item.component.scss']
})
export class VideoListItemComponent {
  @Input() video:VideoListItemDto

}

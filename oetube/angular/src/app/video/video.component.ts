import { Component,OnInit } from '@angular/core';
import { VideoService } from '@proxy/application';
import { VideoListItemDto } from '@proxy/application/dtos/videos';
import { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';

import { Observable } from 'rxjs';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.scss'],
})
export class VideoComponent{

}

import { Component,OnInit } from '@angular/core';
import { VideoService } from '@proxy/application';
import { VideoListItemDto } from '@proxy/application/dtos/videos';
import { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';

import { Observable } from 'rxjs';
import { LazyTabItem } from '../lazy-tab-panel/lazy-tab-panel.component';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.scss'],
})
export class VideoComponent{
  inputItems:LazyTabItem[]=[
    {key:"explore",title:"Explore",authRequired:false,isLoaded:true,onlyCreator:false,visible:true},
    {key:"upload",title:"Upload",authRequired:true,isLoaded:true,onlyCreator:false,visible:false},

  ]


}

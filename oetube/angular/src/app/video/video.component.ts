import { Component,OnInit } from '@angular/core';
import { VideoService } from '@proxy/application';
import { VideoItemDto } from '@proxy/application/dtos/videos';
import { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.scss']
})
export class VideoComponent implements OnInit{

  pagedResult?:PagedResultDto<VideoItemDto>
  

  constructor(private readonly videoService:VideoService) {
    
  }
  ngOnInit(): void {
    this.videoService.getList({},{maxResultCount:100}).subscribe(data=>this.pagedResult=data)
  }
}

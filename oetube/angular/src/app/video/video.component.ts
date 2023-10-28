import { Component,OnInit } from '@angular/core';
import { VideoService } from '@proxy/application';
import { VideoListItemDto } from '@proxy/application/dtos/videos';
import { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.scss']
})
export class VideoComponent implements OnInit{

  pagedResult?:PagedResultDto<VideoListItemDto>
  

  constructor(private readonly videoService:VideoService) {
    
  }
  ngOnInit(): void {
    this.videoService.getList({}).subscribe((data)=>{
      this.pagedResult=data
      console.log(this.pagedResult)
    })
  }
}

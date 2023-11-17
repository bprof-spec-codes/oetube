import { Component,Input } from '@angular/core';
import { PlaylistService, VideoService } from '@proxy/application';
import { PaginationDto } from '@proxy/application/dtos';
import { VideoListItemDto, VideoQueryDto } from '@proxy/application/dtos/videos';
import { LoadOptions } from 'devextreme/data';
import { Observable } from 'rxjs';
import { PaginationGridComponent } from '../pagination-grid.component';
import { VideoPaginationGridComponent } from './video-pagination-grid.component';

@Component({
  selector: 'app-video-item-pagination-grid',
  templateUrl: './video-pagination-grid.component.html',
  styleUrls: ['./video-pagination-grid.component.scss']
})

export class VideoItemPaginationGridComponent extends VideoPaginationGridComponent
{
  
  @Input() playlistId:string

  constructor(public videoService:VideoService, public playlistService:PlaylistService){
    super(videoService)
  }
  getList(): Observable<PaginationDto<VideoListItemDto>> {
      return this.playlistService.getVideos(this.playlistId,this.queryArgs)
  }

}

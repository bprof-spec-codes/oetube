import { Component } from '@angular/core';
import { VideoService } from '@proxy/application';
import { PaginationDto } from '@proxy/application/dtos';
import { UpdateVideoDto, VideoDto, VideoListItemDto, VideoQueryDto } from '@proxy/application/dtos/videos';
import { LoadOptions } from 'devextreme/data';
import { Observable } from 'rxjs';
import { PaginationGridComponent } from '../pagination-grid.component';

@Component({
  selector: 'app-video-pagination-grid',
  templateUrl: './video-pagination-grid.component.html',
  styleUrls: ['./video-pagination-grid.component.scss']
})
export class VideoPaginationGridComponent extends PaginationGridComponent<VideoQueryDto,VideoDto,VideoListItemDto,UpdateVideoDto>
{
  constructor(public videoService:VideoService){
    super()
  }
  getList(): Observable<PaginationDto<VideoListItemDto>> {
      return this.videoService.getList(this.queryArgs)
  }

  handleFilter(options: LoadOptions<any>): void {
      
  }

}

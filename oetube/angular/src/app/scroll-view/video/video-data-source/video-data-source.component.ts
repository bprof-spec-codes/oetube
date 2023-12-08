import { Component, forwardRef, Input, ViewChild } from '@angular/core';
import { VideoService } from '@proxy/application';
import { VideoListItemDto } from '@proxy/application/dtos/videos';
import { DataSourceProviderDirective, ScrollViewDataSourceComponent } from '../../scroll-view-data-source/scroll-view-data-source.component';

@Component({
  selector: 'app-video-data-source',
  templateUrl: './video-data-source.component.html',
  styleUrls: ['./video-data-source.component.scss'],
  providers:[{provide:DataSourceProviderDirective,useExisting:forwardRef(()=>VideoDataSourceComponent)}]
})
export class VideoDataSourceComponent extends DataSourceProviderDirective<VideoListItemDto> {
  get scrollViewDataSourceComponent(): ScrollViewDataSourceComponent<VideoListItemDto> {
    return this._scrollViewDataSourceComponent
  }
  @ViewChild(ScrollViewDataSourceComponent) _scrollViewDataSourceComponent:ScrollViewDataSourceComponent<VideoListItemDto>
  constructor(public service:VideoService){
    super()
  }
  @Input() creatorId:string
  getMethod=(args)=>this.service.getList(args)
}

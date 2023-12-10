import { Component, forwardRef, Input, ViewChild } from '@angular/core';
import { PlaylistService, VideoService } from '@proxy/application';
import { VideoListItemDto } from '@proxy/application/dtos/videos';
import { CurrentUserService } from 'src/app/auth/current-user/current-user.service';
import { DataSourceProviderDirective, ScrollViewDataSourceComponent } from 'src/app/scroll-view/scroll-view-data-source/scroll-view-data-source.component';

@Component({
  selector: 'app-playlist-video-data-source',
  templateUrl: './playlist-video-data-source.component.html',
  styleUrls: ['./playlist-video-data-source.component.scss'],
  providers:[{provide:DataSourceProviderDirective,useExisting:forwardRef(()=>PlaylistVideoDataSourceComponent)}]
})
export class PlaylistVideoDataSourceComponent extends DataSourceProviderDirective<VideoListItemDto> {
  @ViewChild(ScrollViewDataSourceComponent) _scrollViewDataSourceComponent:ScrollViewDataSourceComponent<VideoListItemDto>
  get scrollViewDataSourceComponent(): ScrollViewDataSourceComponent<VideoListItemDto>  {
    return this._scrollViewDataSourceComponent
  }
  @Input() playlistId:string
  creatorId:string
  
  constructor(public videoService:VideoService, public playlistService:PlaylistService,public currentUserService:CurrentUserService){
    super()
    this.creatorId=currentUserService.getCurrentUser().id
  }
  
  getMethod=(args)=>this.videoService.getList(args)
  getInitialSelectionMethod=(id,args)=>this.playlistService.getVideos(id,args)

}

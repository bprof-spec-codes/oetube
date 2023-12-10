import { Component, forwardRef, Input, ViewChild } from '@angular/core';
import { PlaylistService } from '@proxy/application';
import { PlaylistItemDto } from '@proxy/application/dtos/playlists';
import { VideoListItemDto } from '@proxy/application/dtos/videos';
import { DataSourceProviderDirective, ScrollViewDataSourceComponent } from 'src/app/scroll-view/scroll-view-data-source/scroll-view-data-source.component';

@Component({
  selector: 'app-playlist-data-source',
  templateUrl: './playlist-data-source.component.html',
  styleUrls: ['./playlist-data-source.component.scss'],
  providers:[{provide:DataSourceProviderDirective,useExisting:forwardRef(()=>PlaylistDataSourceComponent)}]
})
export class PlaylistDataSourceComponent extends DataSourceProviderDirective<PlaylistItemDto> {
  get scrollViewDataSourceComponent(): ScrollViewDataSourceComponent<PlaylistItemDto> {
    return this._scrollViewDataSourceComponent
  }
  @ViewChild(ScrollViewDataSourceComponent) _scrollViewDataSourceComponent:ScrollViewDataSourceComponent<PlaylistItemDto>
  constructor(public service:PlaylistService){
    debugger
    super()
  }
  @Input() creatorId:string
  getMethod=(args)=>this.service.getList(args)
}

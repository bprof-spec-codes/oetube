import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { PlaylistService } from '@proxy/application';
import { PlaylistDto } from '@proxy/application/dtos/playlists';
import { VideoListItemDto, VideoQueryDto } from '@proxy/application/dtos/videos';
import { AccessType } from '@proxy/domain/entities/videos';
import { lastValueFrom } from 'rxjs';
import { LazyTabItem } from 'src/app/lazy-tab-panel/lazy-tab-panel.component';

@Component({
  selector: 'app-playlist-view',
  templateUrl: './playlist-view.component.html',
  styleUrls: ['./playlist-view.component.scss']
})
export class PlaylistViewComponent{
 
  _model:PlaylistDto
  @Input() set model(v:PlaylistDto){
    this._model=v
    if(v!=undefined){
      this.service.getVideos(v.id,{pagination:{skip:0,take:(2**31)-1}}).subscribe(r=>{
        this.videos=[]
        this.videos.push(...r.items)
      })
    }
  }
  get model():PlaylistDto{
    return this._model
  }

  videos: VideoListItemDto[] = []
  constructor(private service:PlaylistService){
  }
 
}

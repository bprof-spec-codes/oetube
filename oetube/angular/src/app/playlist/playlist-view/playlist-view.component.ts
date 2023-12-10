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
export class PlaylistViewComponent implements OnInit {
  inputItems:LazyTabItem[]=[
    {key:"details",title:"Details",authRequired:false,onlyCreator:false,isLoaded:true,visible:true},
    {key:"edit",title:"Edit",authRequired:true,onlyCreator:true,isLoaded:false,visible:true}
  ]
  id:string
  model:PlaylistDto



  videoQuery: VideoQueryDto = {
    pagination:{skip:0,take:(2**31)-1}
  }
  
  videos: VideoListItemDto[] = []
  constructor(private service:PlaylistService,private route:ActivatedRoute){
  }
 async ngOnInit() {
    this.route.paramMap.subscribe((params:ParamMap)=>{
      this.id=params.get('id')
      this.service.get(this.id).subscribe(r=>{
        this.model=r
        this.service.getVideos(this.id,this.videoQuery).subscribe(r=>{
          this.videos.push(...r.items)
        })
      })
    })
  }
}

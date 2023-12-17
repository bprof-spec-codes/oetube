import { Component, OnInit,ViewChild, ChangeDetectorRef } from '@angular/core';
import { PlaylistService, VideoService } from '@proxy/application';
import { VideoDto } from '@proxy/application/dtos/videos';
import { LazyTabItem } from 'src/app/lazy-tab-panel/lazy-tab-panel.component';
import{ ActivatedRoute, Router } from "@angular/router"
import { PlaylistDto } from '@proxy/application/dtos/playlists';
import { VideoPlayerComponent } from '../video-player/video-player.component';
import { VideoTimeService } from 'src/app/services/video/video-time.service';
import { VideoPlayerService } from 'src/app/services/video/video-player.service';
@Component({
  selector: 'app-video-details',
  templateUrl: './video-details.component.html',
  styleUrls: ['./video-details.component.scss']
})
export class VideoDetailsComponent {
  inputItems:LazyTabItem[]=[
    {key:"watch",title:"Watch",authRequired:false,onlyCreator:false,isLoaded:true,visible:true},
    {key:"edit",title:"Edit",authRequired:true,onlyCreator:true,isLoaded:false,visible:true},
  ]

  model:VideoDto
  playlist:PlaylistDto  
  getRoute:Function
  getMethod:Function
  @ViewChild(VideoPlayerComponent) player:VideoPlayerComponent

  pause(){
    this.player?.wrapper?.videoPlayerService.pause()
  }
  reset(){
    if(this.player?.wrapper)
    {
      this.player.wrapper.videoPlayerService.pause()
      this.player.wrapper.videoTimeService.setCurrentTime(0)
      this.player.wrapper.videoTimeService.setVideoDuration(0)
      this.player.wrapper.videoTimeService.setVideoProgress(0)
      this.changeDetector.detectChanges()

    }

  }

  constructor(private videoService:VideoService,private changeDetector:ChangeDetectorRef,private playlistService:PlaylistService, private route:ActivatedRoute,private router:Router){
    this.route.paramMap.subscribe(p=>{
      const id=p.get("id")
      this.videoService.get(id).subscribe(v=>{
        this.model=v
      const playlistId=p.get("playlistId")
      if(playlistId){
        this.playlistService.get(playlistId).subscribe(p=>{
          this.playlist=p
          this.getMethod=(args)=>this.playlistService.getVideos(this.playlist.id,args)
          this.getRoute=(id)=>["/video",id,this.playlist.id]
         this.reset()
        })
      }
      else{
        this.getMethod=(args)=>this.videoService.getList(args)
        this.getRoute=(id)=>["/video",id]
        this.reset()
      }
    })
    })
  }
onSelectedItemChange(e){
  if(e.key=="edit"){
    this.pause()
  }
}
onDeleted(){
  this.router.navigate(["video"])  
}

}

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlaylistService } from '@proxy/application';
import { PlaylistDto } from '@proxy/application/dtos/playlists';
import { VideoListItemDto, VideoQueryDto } from '@proxy/application/dtos/videos';
import { AccessType } from '@proxy/domain/entities/videos';

@Component({
  selector: 'app-playlist-view',
  templateUrl: './playlist-view.component.html',
  styleUrls: ['./playlist-view.component.scss']
})
export class PlaylistViewComponent implements OnInit {

  route: ActivatedRoute

  service: PlaylistService
  playlist: PlaylistDto
  videoQuery: VideoQueryDto = {
    pagination:{skip:0,take:2<<32}
  }
  videos: VideoListItemDto[] = []

  constructor(service: PlaylistService, route: ActivatedRoute){
    this.service = service
    this.route = route
  }

  ngOnInit(): void {
    let id = ""
    this.route.params.subscribe(param => {
      id = param['id']

      //Playlist lekérése
      this.service.get(id).subscribe(x => {
        this.playlist = x

        //Playlisthez tartozó videók lekérése
        this.service.getVideos(id, this.videoQuery).subscribe(resp=> {
          resp.items.forEach(z=> {
            z.duration = z.duration.split('.')[0]
            this.videos.push(z)
          })
        })
      })
    })
  }
}

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
  videoQuery: VideoQueryDto
  videos: VideoListItemDto[] = []
  
  video1: VideoListItemDto = {
    name: "Test video",
    indexImage: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR7-Rmh977-3igeJeqFnYtR-X5AtzLc3eZ0hg&usqp=CAU",
    duration: "5:00",
    creationTime: "2023.05.12",
    playlistId: "",
    access: AccessType.Public,
    creator: null
  }

  video2: VideoListItemDto = {
    name: "Some video",
    indexImage: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR7-Rmh977-3igeJeqFnYtR-X5AtzLc3eZ0hg&usqp=CAU",
    duration: "15:00",
    creationTime: "2023.12.12",
    playlistId: "",
    access: AccessType.Public,
    creator: null
  }

  testPlaylist: PlaylistDto = {
    name: "Teszt playlist",
    description: "This is a long description about the playlist which is used for testing",
    creationTime: "2023.12.07",
    image: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR7-Rmh977-3igeJeqFnYtR-X5AtzLc3eZ0hg&usqp=CAU",
    items: [],
    creator: null,
    totalDuration: ""
  }

  constructor(service: PlaylistService, route: ActivatedRoute){
    this.service = service
    this.route = route
  }

  ngOnInit(): void {
    this.videos.push(this.video1)
    this.videos.push(this.video2)
    let id = "cb62d735-5258-e2b2-b315-3a0f5738464d"
    this.route.params.subscribe(param => {
      id = param['id']

      //Playlist lekérése
      this.service.get(id).subscribe(x => {
        this.playlist = x

        //Playlisthez tartozó videók lekérése
        this.service.getVideos(id, this.videoQuery).subscribe(resp=> {
          //resp.items
          //resp.items.forEach(x => x.)
        })
      })
    })
  }
}

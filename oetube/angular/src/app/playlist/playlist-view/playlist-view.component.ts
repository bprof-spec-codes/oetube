import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlaylistService } from '@proxy/application';
import { PlaylistDto } from '@proxy/application/dtos/playlists';

@Component({
  selector: 'app-playlist-view',
  templateUrl: './playlist-view.component.html',
  styleUrls: ['./playlist-view.component.scss']
})
export class PlaylistViewComponent implements OnInit {

  route: ActivatedRoute

  service: PlaylistService
  playlist: PlaylistDto


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
    let id = "cb62d735-5258-e2b2-b315-3a0f5738464d"
    this.route.params.subscribe(param => {
      id = param['id']
      this.service.get(id).subscribe(x => {
        this.playlist = x
      })
    })
  }
}

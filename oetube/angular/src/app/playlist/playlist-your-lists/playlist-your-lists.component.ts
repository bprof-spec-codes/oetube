import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserDto } from '@proxy/application/dtos/oe-tube-users/models';
import { PlaylistDto, PlaylistItemDto, PlaylistQueryDto } from '@proxy/application/dtos/playlists';
import { OeTubeUserService } from '@proxy/application/oe-tube-user.service';
import { PlaylistService } from '@proxy/application/playlist.service';
import { ConfigStateService } from '@abp/ng.core';


@Component({
  selector: 'app-playlist-your-lists',
  templateUrl: './playlist-your-lists.component.html',
  styleUrls: ['./playlist-your-lists.component.scss']
})
export class PlaylistYourListsComponent implements OnInit {

  playlists : Array<PlaylistItemDto>
  playlistQueryDto : PlaylistQueryDto =
  {
    pagination : {
      skip: 0,
      take: 50,
    }
  }
  userData : any = {}
  itemTemplate(itemData: any, index: number, element: any){
    let ownerClass = itemData.creator.id === this.userData.id.toString() ? 'owned-playlist' : ''
    element.innerHTML = `<div class="${ownerClass}">${itemData.name}</div>`;
  }
  
  constructor(private playlistService : PlaylistService,
              private userService : OeTubeUserService,
              private router : Router,
              private config : ConfigStateService) {
  }

  ngOnInit(): void {
    this.playlistService.getList(this.playlistQueryDto).subscribe({
      next: success => {
        console.log(success)
        this.playlists = success.items
      }
    })
    this.userData = this.config.getOne("currentUser")
  }

  navigateToList(event){
    let itemData : any = event.itemData
    console.log(itemData)
    //the route may vary according to the creator of the playlist content component
    this.router.navigate(['playlist/contents/' + itemData.id])
  }

  deletePlaylist(event){
    let itemData : any = event.itemData
    this.playlistService.delete(itemData.id)
  }
}

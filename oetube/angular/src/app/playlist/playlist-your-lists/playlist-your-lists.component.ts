import { Component, OnInit } from '@angular/core';
import { PlaylistDto, PlaylistItemDto, PlaylistQueryDto } from '@proxy/application/dtos/playlists';
import { PlaylistService } from '@proxy/application/playlist.service';
import { ItemPerPage } from '@proxy/domain/repositories/query-args';

@Component({
  selector: 'app-playlist-your-lists',
  templateUrl: './playlist-your-lists.component.html',
  styleUrls: ['./playlist-your-lists.component.scss']
})
export class PlaylistYourListsComponent implements OnInit {

  playlists : Array<PlaylistItemDto>
  playlistQueryDto : PlaylistQueryDto =
  {
    itemPerPage:ItemPerPage.P50,
    page:0
  }
  
  constructor(private playlistService : PlaylistService) {
  }

  ngOnInit(): void {
    this.playlistService.getList(this.playlistQueryDto).subscribe({
      next: success => {
        console.log(success)
        this.playlists = success.items
      }
    })
  }

  navigateToList(event){
    console.log(event.itemData)
  }
}

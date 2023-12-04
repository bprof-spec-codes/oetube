import { Component } from '@angular/core';
import { PlaylistDto } from '@proxy/application/dtos/playlists';

@Component({
  selector: 'app-playlist-your-lists',
  templateUrl: './playlist-your-lists.component.html',
  styleUrls: ['./playlist-your-lists.component.scss']
})
export class PlaylistYourListsComponent {
  playlists : Array<PlaylistDto>
}

import { Component } from '@angular/core';
import { PlaylistDto } from '@proxy/application/dtos/playlists';

@Component({
  selector: 'app-playlist-player',
  templateUrl: './playlist-player.component.html',
  styleUrls: ['./playlist-player.component.scss']
})
export class PlaylistPlayerComponent {
  playlistId?: string;
  public playlist?: PlaylistDto;
}

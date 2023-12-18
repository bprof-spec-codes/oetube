import { Component,Input, OnDestroy, OnInit } from '@angular/core';
import { PlaylistItemDto } from '@proxy/application/dtos/playlists';
import { UrlService } from 'src/app/current-user/url-service/url.service';

@Component({
  selector: 'app-playlist-list-item',
  templateUrl: './playlist-list-item.component.html',
  styleUrls: ['./playlist-list-item.component.scss']
})
export class PlaylistListItemComponent{
  @Input() item:PlaylistItemDto 

}

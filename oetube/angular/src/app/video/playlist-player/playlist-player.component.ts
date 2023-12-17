import { Component,Input } from '@angular/core';
import { PlaylistDto } from '@proxy/application/dtos/playlists';
import { ActivatedRoute } from '@angular/router';
import { PlaylistService } from '@proxy/application';
import { VideoDto, VideoListItemDto } from '@proxy/application/dtos/videos';
import { ScrollViewContent } from 'src/app/scroll-view/scroll-view-contents/scroll-view-contents.component';

@Component({
  selector: 'app-playlist-player',
  templateUrl: './playlist-player.component.html',
  styleUrls: ['./playlist-player.component.scss']
})
export class PlaylistPlayerComponent {

  inputItems:ScrollViewContent[]=[
    {key:"tile",hint:"Tile",icon:"rowfield",layoutClassList:"d-flex flex-column  justify-content-center"},
  ]
  @Input() currentVideo:VideoDto
  @Input() getRoute:(v:VideoListItemDto)=>string[]
  @Input() getMethod:Function

}

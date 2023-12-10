import { Component, Input } from '@angular/core';
import { PlaylistDto } from '@proxy/application/dtos/playlists';

@Component({
  selector: 'app-playlist-editor',
  templateUrl: './playlist-editor.component.html',
  styleUrls: ['./playlist-editor.component.scss']
})
export class PlaylistEditorComponent {
  @Input() model:PlaylistDto
}
@Component({
  selector:'app-playlist-update',
  templateUrl: './playlist-editor.component.html',
  styleUrls: ['./playlist-editor.component.scss']
})
export class PlaylistUpdateComponent extends PlaylistEditorComponent{
  
}
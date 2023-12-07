import { Component, Input, TemplateRef, } from '@angular/core';
import { CreateUpdatePlaylistDto, PlaylistDto } from '@proxy/application/dtos/playlists';
import { CreatorDto } from '@proxy/application/dtos/oe-tube-users'
import { PlaylistService } from '@proxy/application/playlist.service';
import { UserDto } from '@proxy/application/dtos/oe-tube-users';
import { NestedOptionHost } from 'devextreme-angular';

@Component({
  selector: 'app-playlist-create',
  templateUrl: './playlist-create.component.html',
  styleUrls: ['./playlist-create.component.scss'],

})
export class PlaylistCreateComponent {
  playlistService : PlaylistService

  submitButtonOptions={
    text:"Create",
    useSubmitBehavior:true,
    type:"default"
  }

  acceptedFileTypes: string = '.jpg, .jpeg, .png, .gif'

  constructor(playlistService : PlaylistService) {
    this.playlistService = playlistService
  }

  playlistModel : CreateUpdatePlaylistDto = {
    name: '',
    description: '',
    items: [],
    image: new FormData()
  }

  onSubmit(event:Event) {
    event.preventDefault();
    console.log(this.playlistModel)
    //this.playlistService.create(this.playlistModel)
  }

  onFileSelected(event){
    this.playlistModel.image = event.value;
  }
}

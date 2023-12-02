import { Component } from '@angular/core';
import { PlaylistDto } from '@proxy/application/dtos/playlists';
import { CreatorDto } from '@proxy/application/dtos/oe-tube-users'

@Component({
  selector: 'app-playlist-create',
  templateUrl: './playlist-create.component.html',
  styleUrls: ['./playlist-create.component.scss']
})
export class PlaylistCreateComponent {
  submitButtonOptions={
    text:"Submit",
    useSubmitBehavior:true,
    type:"normal"
  }

  constructor() {
  }

  playlistModel : PlaylistDto = {
    name: '',
    description: '',
    creationTime: '',
    items: [],
    image: '',
    creator: {
      name: '',
      thumbnailImage: '',
      currentUserIsCreator: true
    } ,
    totalDuration: ''
  }

  onSubmit(event:Event) {
    event.preventDefault();
    console.log(this.playlistModel)

  }
}

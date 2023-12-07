import { Component, Input, TemplateRef, } from '@angular/core';
import { CreateUpdatePlaylistDto, PlaylistDto } from '@proxy/application/dtos/playlists';
import { CreatorDto } from '@proxy/application/dtos/oe-tube-users'
import { PlaylistService } from '@proxy/application/playlist.service';
import { UserDto } from '@proxy/application/dtos/oe-tube-users';
import { NestedOptionHost } from 'devextreme-angular';
import { error } from 'console';
import { Router } from '@angular/router';

@Component({
  selector: 'app-playlist-create',
  templateUrl: './playlist-create.component.html',
  styleUrls: ['./playlist-create.component.scss'],

})
export class PlaylistCreateComponent {
  submitButtonOptions={
    text:"Create",
    useSubmitBehavior: true,
    type:"default"
  }

  playlistModel : CreateUpdatePlaylistDto = {
    name: '',
    description: '',
    items: [],
    image: new FormData()
  }

  successfulPlaylistCreation = {
    CreatorName : '',
    Name : '',
    Description : '',
    Image : ''
  }

  acceptedFileTypes: string = '.jpg, .jpeg, .png, .gif'

  isPopupVisible = false

  constructor(private playlistService : PlaylistService,
              private router : Router,) {
  }

  togglePopup(){
    this.isPopupVisible = !this.isPopupVisible
  }


  onSubmit(event:Event) {
    event.preventDefault();
    this.playlistService.create(this.playlistModel).subscribe({
      next : success => {
        console.log(success)
        this.successfulPlaylistCreation.CreatorName = success.creator.name
        this.successfulPlaylistCreation.Name = success.name
        this.successfulPlaylistCreation.Description = success.description
        this.successfulPlaylistCreation.Image = success.image

        this.isPopupVisible = !this.isPopupVisible
      }
    })
  }

  navigateToPlaylists(){
    this.router.navigate(['playlist/playlists'])
  }

  onFileSelected(event){
    this.playlistModel.image = event.value;
  }
}

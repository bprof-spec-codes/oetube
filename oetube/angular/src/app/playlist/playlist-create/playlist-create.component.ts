import { Component, Input, OnInit, TemplateRef, } from '@angular/core';
import { CreateUpdatePlaylistDto, PlaylistDto } from '@proxy/application/dtos/playlists';
import { PlaylistService } from '@proxy/application/playlist.service';
import { Router } from '@angular/router';
import { VideoService } from '@proxy/application';
import { VideoQueryDto } from '@proxy/application/dtos/videos';

@Component({
  selector: 'app-playlist-create',
  templateUrl: './playlist-create.component.html',
  styleUrls: ['./playlist-create.component.scss'],

})
export class PlaylistCreateComponent{
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

  videosToAdd : any = {}

  acceptedFileTypes: string = '.jpg, .jpeg, .png, .gif'

  isPopupVisible = false

  constructor(private playlistService : PlaylistService,
              private router : Router,
              private videoService : VideoService) {
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

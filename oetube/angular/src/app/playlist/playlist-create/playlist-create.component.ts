import { Component, Input, OnInit, TemplateRef, } from '@angular/core';
import { CreateUpdatePlaylistDto, PlaylistDto } from '@proxy/application/dtos/playlists';
import { PlaylistService } from '@proxy/application/playlist.service';
import { Router } from '@angular/router';
import { VideoService } from '@proxy/application';

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
    this.playlistService.create(this.playlistModel).subscribe(r=>{
      this.router.navigate(['/playlist',r.id])
    })
  }

}
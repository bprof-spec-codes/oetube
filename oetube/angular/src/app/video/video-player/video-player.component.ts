import { Component, Input, OnChanges, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Observable, Subscription } from 'rxjs';

import { ActivatedRoute } from '@angular/router';
import {
  VideoService as VideoAppService,
  PlaylistService
} from '@proxy/application';
import { VideoDto, VideoListItemDto } from '@proxy/application/dtos/videos';
import { PlaylistDto } from '@proxy/application/dtos/playlists';
import { VideoWrapperComponent } from './video-wrapper/video-wrapper.component';

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss'],
})
export class VideoPlayerComponent  {
  @Input() video?: VideoDto;

  @ViewChild(VideoWrapperComponent) wrapper:VideoWrapperComponent

  public resolutionIndex: number = 0;
  onResolutionChanged(index: number) {
    this.resolutionIndex = index;
  }

}

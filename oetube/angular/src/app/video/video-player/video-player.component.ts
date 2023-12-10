import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { Observable, Subscription } from 'rxjs';

import { ActivatedRoute } from '@angular/router';
import {
  VideoService as VideoAppService,
  PlaylistService
} from '@proxy/application';
import { VideoDto, VideoListItemDto } from '@proxy/application/dtos/videos';
import { PlaylistDto } from '@proxy/application/dtos/playlists';
import { VideoService } from 'src/app/services/video/video.service';

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss'],
})
export class VideoPlayerComponent implements OnInit, OnDestroy {
  id: string;
  private sub: Subscription;
  public videoUrl: string;

  playlistId?: string;
  public playlist?: PlaylistDto;

  @Input() video?: VideoDto;

  public resolutionIndex: number = 0;

  constructor(
    private route: ActivatedRoute,
    private playlistService: PlaylistService
  ) {}

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];
      this.playlistId = params['playlist']
      if (this.playlistId) {
        this.playlistService.get(this.playlistId).subscribe(data => {
          this.playlist = data
        })
      }
    });
  }

  onResolutionChanged(index: number) {
    this.resolutionIndex = index;
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}

import { Component, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
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

  public playlist?: PlaylistDto;
  public playlistVideos?: VideoListItemDto[];

  public video?: VideoDto;
  public resolutionIndex: number = 0;

  constructor(
    private route: ActivatedRoute,
    private videoAppService: VideoAppService,
    private playlistService: PlaylistService
  ) {}

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];
      this.videoAppService.get(this.id).subscribe(data => {
        this.video = data;
        if (this.video.playlistId) {
          this.playlistService.get(this.video.playlistId).subscribe(data => {
            this.playlist = data
            this.playlistService.getVideos(
              this.playlist.id,
              { pagination: { take: 100, skip: 0 } }
            ).subscribe(data => {
              this.playlistVideos = data.items
            })
          })
        }
      });
    });
  }

  onResolutionChanged(index: number) {
    this.resolutionIndex = index;
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}

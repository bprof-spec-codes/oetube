import { Component, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { Observable, Subscription } from 'rxjs';

import { ActivatedRoute } from '@angular/router';
import { VideoService as VideoAppService } from '@proxy/application';
import { VideoDto } from '@proxy/application/dtos/videos';
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

  public video?: VideoDto;
  public resolutionIndex: number = 0;

  constructor(
    private route: ActivatedRoute,
    private videoService: VideoService,
    private appService: VideoAppService
  ) {}

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];
      this.appService.get(this.id).subscribe(data => {
        this.video = data;
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

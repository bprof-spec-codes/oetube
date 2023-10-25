import { Component, OnDestroy, OnInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
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

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}

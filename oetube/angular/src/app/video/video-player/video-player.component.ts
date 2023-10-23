import { Component, OnDestroy, OnInit } from '@angular/core';

import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Video } from '../video-grid/video-grid/video-grid.component';
import { VideoService } from 'src/app/services/video/video.service';

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss'],
})
export class VideoPlayerComponent implements OnInit, OnDestroy {
  id: string;
  private sub: Subscription;
  public video: Video;
  public videoUrl: string;

  constructor(private route: ActivatedRoute, private videoService: VideoService) {}

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];

      this.video = {
        Id: '1',
        Name: 'Test video',
        Description: 'Placeholder description',
        Duration: 5000,
        CreationTime: new Date(),
        CreatorId: 'guid',
      };
      this.videoUrl =
        'https://bitdash-a.akamaihd.net/content/MI201109210084_1/m3u8s/f08e80da-bf1d-4e3d-8899-f0f6155f6efa.m3u8';
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}

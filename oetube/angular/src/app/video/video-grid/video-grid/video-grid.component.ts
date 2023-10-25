import { Component, Input, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { PagedResultDto, PagedResultRequestDto } from '@abp/ng.core';

import { VideoItemDto } from '@proxy/application/dtos/videos';
import { VideoService } from '@proxy/application';

@Component({
  selector: 'app-video-grid',
  templateUrl: './video-grid.component.html',
  styleUrls: ['./video-grid.component.scss'],
})
export class VideoGridComponent implements OnInit {
  public videos: VideoItemDto[];
  public isLoading: boolean;

  constructor(private readonly videoService: VideoService) {}

  ngOnInit(): void {
    this.refreshVideos();
  }

  onSearch(searchPhrase: string) {
    this.refreshVideos(searchPhrase);
  }

  refreshVideos(searchPhrase?: string) {
    //Observable from Create Method
    this.videos = undefined;
    this.isLoading = true;

    this.videoService
      .getList({}, { maxResultCount: 100 })
      .subscribe(data => (this.videos = data.items));

    this.isLoading = false;
  }

  fromatVideoDuration(duration: string) {
    return duration.split('.')[0];
  }
}

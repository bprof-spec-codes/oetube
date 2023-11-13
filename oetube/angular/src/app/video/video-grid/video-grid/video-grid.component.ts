import { Component, Input, OnInit } from '@angular/core';

import { VideoListItemDto } from '@proxy/application/dtos/videos';
import { VideoService } from '@proxy/application';
import { ItemPerPage } from '@proxy/domain/repositories/query-args';

@Component({
  selector: 'app-video-grid',
  templateUrl: './video-grid.component.html',
  styleUrls: ['./video-grid.component.scss'],
})
export class VideoGridComponent implements OnInit {
  public videos: VideoListItemDto[];
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
      .getList({name:searchPhrase,itemPerPage:ItemPerPage.P50,page:0})
      .subscribe(data => (this.videos = data.items));

    this.isLoading = false;
  }

  fromatVideoDuration(duration: string) {
    return duration.split('.')[0];
  }
}

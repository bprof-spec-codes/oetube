import { Component, Input, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { PagedResultDto, PagedResultRequestDto } from '@abp/ng.core';

import { VideoListItemDto } from '@proxy/application/dtos/videos';
import { VideoService, PlaylistService } from '@proxy/application';

@Component({
  selector: 'app-video-grid',
  templateUrl: './video-grid.component.html',
  styleUrls: ['./video-grid.component.scss'],
})
export class VideoGridComponent implements OnInit {
  public videos: VideoListItemDto[];
  public isLoading: boolean;

  @Input() multicolumn?: boolean;

  @Input() currentVideoId?: string;
  @Input() currentPlaylistId?: string;

  public rowClasses: string[] = [];

  constructor(
    private readonly videoService: VideoService,
    private readonly playlistService: PlaylistService
  ) {}

  ngOnInit(): void {
    this.refreshVideos();

    this.rowClasses = !this.multicolumn
      ? ['row', 'gx-5', 'gy-4', 'row-cols-1', 'row-cols-md-3', 'row-cols-lg-4']
      : ['row', 'row-cols-1'];
  }

  onSearch(searchPhrase: string) {
    this.refreshVideos(searchPhrase);
  }

  refreshVideos(searchPhrase?: string) {
    //Observable from Create Method
    this.videos = undefined;
    this.isLoading = true;

    if (this.currentPlaylistId) {
      this.playlistService
        .getVideos(this.currentPlaylistId, { name: searchPhrase, pagination: { take: 100, skip: 0 } })
        .subscribe(data => (this.videos = data.items));
    } else {
      this.videoService
        .getList({ name: searchPhrase, pagination: { take: 100, skip: 0 } })
        .subscribe(data => (this.videos = data.items));
    }

    this.isLoading = false;
  }

  fromatVideoDuration(duration: string) {
    return duration.split('.')[0];
  }
}

import { BehaviorSubject, Observable } from 'rxjs';

import { Injectable } from '@angular/core';
import { VideoService } from '../video/video.service';

export type PlaylistItem = {
  /**
   * Item title.
   */
  title: string;

  /**
   * Item url.
   */
  url: string;
};

@Injectable({
  providedIn: 'root',
})
export class VideoPlaylistService {
  private list = new BehaviorSubject<PlaylistItem[]>([]);
  private currentVideo = new BehaviorSubject<string>('');
  private shouldPlayNext = new BehaviorSubject<boolean>(true);

  constructor(private videoService: VideoService) {}

  setShouldPlayNext(playNext: boolean): void {
    this.shouldPlayNext.next(playNext);
  }

  get shouldPlayNext$(): Observable<boolean> {
    return this.shouldPlayNext.asObservable();
  }

  get list$(): Observable<PlaylistItem[]> {
    return this.list.asObservable();
  }

  get currentVideo$(): Observable<string> {
    return this.currentVideo.asObservable();
  }

  setCurrentVideo(video: string): void {
    if (this.currentVideo.getValue() !== video) {
      this.currentVideo.next(video);
      this.videoService.pause();
    }
  }

  playNextVideo(): void {
    const activeIndex = this.list
      .getValue()
      .findIndex(video => this.currentVideo.getValue() === video.url);
    this.setCurrentVideoByIndex(activeIndex + 1);
  }

  setCurrentVideoByIndex(index: number): void {
    this.setCurrentVideo(this.list.getValue()[index < this.list.getValue().length ? index : 0].url);
  }

  fetchList(endpoint: string): void {
    fetch(endpoint)
      .then(response => {
        return [
          {
            title: 'Playhouse VR',
            url: 'https://bitmovin-a.akamaihd.net/content/playhouse-vr/m3u8s/105560.m3u8',
          },
        ];
      })
      .then((playlist: PlaylistItem[]) => {
        this.list.next(playlist);
        this.setCurrentVideo(playlist[0].url);
      });
  }
}

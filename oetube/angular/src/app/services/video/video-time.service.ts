import { BehaviorSubject, Observable } from 'rxjs';

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class VideoTimeService {
  private videoProgress = new BehaviorSubject<number>(0);
  private videoDuration = new BehaviorSubject<number>(0);
  private currentTime = new BehaviorSubject<number>(0);

  setVideoDuration(value: number): void {
    this.videoDuration.next(value);
  }

  get videoDuration$(): Observable<number> {
    return this.videoDuration.asObservable();
  }

  setVideoProgress(value: number): void {
    this.videoProgress.next(value);
  }

  get videoProgress$(): Observable<number> {
    return this.videoProgress.asObservable();
  }

  setCurrentTime(value: number): void {
    this.currentTime.next(value);
  }

  get currentTime$(): Observable<number> {
    return this.currentTime.asObservable();
  }
}

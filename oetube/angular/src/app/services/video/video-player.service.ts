import { BehaviorSubject, Observable, Subject } from 'rxjs';

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class VideoPlayerService {
  private playingState = new Subject<boolean>();
  private loading = new BehaviorSubject<boolean>(true);
  private videoEnded = new BehaviorSubject<boolean>(false);

  get loading$(): Observable<boolean> {
    return this.loading.asObservable();
  }

  setLoading(value: boolean): void {
    this.loading.next(value);
  }

  play(): void {
    this.playingState.next(true);
  }

  pause(): void {
    this.playingState.next(false);
  }

  get playingState$(): Observable<boolean> {
    return this.playingState.asObservable();
  }

  get videoEnded$(): Observable<boolean> {
    return this.videoEnded.asObservable();
  }

  setVideoEnded(value: boolean): void {
    this.videoEnded.next(value);
  }
}

import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  OnChanges,
  OnDestroy,
  SimpleChanges,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { HlsResolutionDto, VideoDto } from '@proxy/application/dtos/videos';
import Hls from 'hls.js';
import { VideoTimeService } from 'src/app/services/video/video-time.service';
import { VolumeService } from 'src/app/services/video/volume.service';
import { VideoPlayerService } from 'src/app/services/video/video-player.service';
import { CurrentUserService } from 'src/app/current-user/services/current-user.service';
@Component({
  selector: 'app-video-wrapper',
  templateUrl: './video-wrapper.component.html',
  styleUrls: ['./video-wrapper.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class VideoWrapperComponent implements AfterViewInit, OnDestroy, OnChanges {
  loading = true;
  playing = false;
  playNext = true;
  private videoEnded = false;
  private hls:Hls
  private videoListeners = {
    loadedmetadata: () =>
      this.videoTimeService.setVideoDuration(this.videoElement.nativeElement.duration),
    canplay: () => this.videoPlayerService.setLoading(false),
    seeking: () => this.videoPlayerService.setLoading(true),
    timeupdate: () => {
      this.videoTimeService.setVideoProgress(this.videoElement.nativeElement.currentTime);
      if (
        this.videoElement.nativeElement.currentTime === this.videoElement.nativeElement.duration &&
        this.videoElement.nativeElement.duration > 0
      ) {
        this.videoPlayerService.pause();
        this.videoPlayerService.setVideoEnded(true);
      } else {
        this.videoPlayerService.setVideoEnded(false);
      }
    },
  };

  @Input() resolutionIndex: number = 0;

  @Input() video: VideoDto;

  @ViewChild('video', { static: true }) videoElement: ElementRef<HTMLVideoElement> =
    {} as ElementRef<HTMLVideoElement>;

  constructor(
    public videoPlayerService: VideoPlayerService,
    public volumeService: VolumeService,
    public videoTimeService: VideoTimeService,
    currentUserService:CurrentUserService,
  ) {
    this.hls=new Hls({
      xhrSetup(xhr, url) {
        if(currentUserService.getCurrentUser().isAuthenticated){
          xhr.withCredentials=true
          const [header,value]=currentUserService.getAuthorizationHeaderValue()
          xhr.setRequestHeader(header,value)
        }
      }
    })
  }

  pause(){
    this.videoElement.nativeElement.pause()
  }
  ngAfterViewInit() {
    this.subscriptions();
    Object.keys(this.videoListeners).forEach(videoListener => {
      if (this.videoElement) {
        this.videoElement.nativeElement.addEventListener(
          videoListener,
          this.videoListeners[videoListener as keyof typeof this.videoListeners]
        );
      }
    });
    this.load(this.video.hlsResolutions[0].hlsList);
  }

  ngOnDestroy() {
    Object.keys(this.videoListeners).forEach(videoListener => {
      if (this.videoElement) {
        this.videoElement.nativeElement.removeEventListener(
          videoListener,
          this.videoListeners[videoListener as keyof typeof this.videoListeners]
        );
      }
    });
    this.hls.detachMedia();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.video) {
      console.log(changes)
      const currentTime = this.videoElement.nativeElement.currentTime;

      this.load(this.video.hlsResolutions[this.resolutionIndex].hlsList);
      this.videoTimeService.setCurrentTime(currentTime);
      if(changes["video"]==undefined){
        this.videoPlayerService.play()
      }
    }
  }

  /** Play/Pause video on click */
  onVideoClick() {
    if (this.playing) {
      this.videoPlayerService.pause();
    } else {
      this.videoPlayerService.play();
    }
  }

  /** Go full screen on double click */
  onDoubleClick() {
    if (document.fullscreenElement) {
      document.exitFullscreen();
    } else {
      const videoPlayerDiv = document.querySelector('.video-player');
      if (videoPlayerDiv?.requestFullscreen) {
        videoPlayerDiv.requestFullscreen();
      }
    }
  }

  /**
   * Loads the video, if the browser supports HLS then the video use it, else play a video with native support
   */
  load(currentVideo: string): void {
    this.videoTimeService.setVideoProgress(0);
    this.videoTimeService.setCurrentTime(0);
    if (Hls.isSupported()) {
      this.loadVideoWithHLS(currentVideo);
    } else {
      if (this.videoElement.nativeElement.canPlayType('application/vnd.apple.mpegurl')) {
        this.loadVideo(currentVideo);
      }
    }
  }

  get videoIconPlaying() {
    return this.videoEnded && !this.playNext
      ? {
          name: 'Replay',
          value: 'replay',
        }
      : this.playing
      ? {
          name: 'Pause',
          value: 'pause',
        }
      : {
          name: 'Play',
          value: 'play_arrow',
        };
  }

  /**
   * Play or Pause current video
   */
  private playPauseVideo(playing: boolean) {
    this.playing = playing;
    this.videoElement.nativeElement[playing ? 'play' : 'pause']();
  }

  /**
   * Setup subscriptions
   */
  private subscriptions() {
    this.videoPlayerService.playingState$.subscribe(playing => this.playPauseVideo(playing));
    this.videoTimeService.currentTime$.subscribe(
      currentTime => (this.videoElement.nativeElement.currentTime = currentTime)
    );
    this.volumeService.volumeValue$.subscribe(
      volume => (this.videoElement.nativeElement.volume = volume)
    );
    this.videoPlayerService.videoEnded$.subscribe(ended => (this.videoEnded = ended));
    this.videoPlayerService.loading$.subscribe(loading => (this.loading = loading));
  }

  /**
   * Method that loads the video with HLS support
   */
  private loadVideoWithHLS(currentVideo: string) {
    this.hls.loadSource(currentVideo);
    this.hls.attachMedia(this.videoElement.nativeElement);
  }

  /**
   * Method that loads the video without HLS support
   */
  private loadVideo(currentVideo: string) {
    this.videoElement.nativeElement.src = currentVideo;
  }
}

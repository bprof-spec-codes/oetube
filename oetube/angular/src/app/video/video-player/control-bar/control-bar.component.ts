import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';

import { ValueChangedEvent } from 'devextreme/ui/progress_bar';
import { VideoDto } from '@proxy/application/dtos/videos';
import { VideoService } from 'src/app/services/video/video.service';
import { VideoTimeService } from 'src/app/services/video/video-time.service';

@Component({
  selector: 'app-control-bar',
  templateUrl: './control-bar.component.html',
  styleUrls: ['./control-bar.component.scss'],
})
export class ControlBarComponent implements OnInit {
  playing = false;
  currentProgress = 0;
  duration = 0;
  currentTime = 0;
  label = 'Audio volume';
  private videoEnded = false;
  @Input() video?: VideoDto;
  @Input() resolutionIndex: number;
  @Output() resolutionChanged: EventEmitter<number> = new EventEmitter();
  resolutions = [];
  isDropDownBoxOpened = false;
  selectedResolution = '';

  constructor(private videoService: VideoService, private videoTimeService: VideoTimeService) {}

  ngOnInit() {
    this.videoService.playingState$.subscribe(playing => (this.playing = playing));
    this.videoTimeService.videoDuration$.subscribe(duration => (this.duration = duration));
    this.videoTimeService.videoProgress$.subscribe(progress => (this.currentProgress = progress));
    this.videoService.videoEnded$.subscribe(ended => (this.videoEnded = ended));

    this.videoService.loading$.subscribe(value => {
      if (!value) {
        this.resolutions = this.video?.hlsResolutions.map(r => r.width + 'p');
      }
    });
  }

  onPlayClick() {
    if (this.playing) {
      this.videoService.pause();
    } else {
      this.videoService.play();
    }
  }

  onChange(event: ValueChangedEvent) {
    if (event.event) {
      // The interaction was done by the user
      this.videoTimeService.setVideoProgress(event?.value ?? 0);
      this.videoTimeService.setCurrentTime(event?.value ?? 0);
    }
  }

  onFullscreen() {
    if (document.fullscreenElement) {
      document.exitFullscreen();
    } else {
      const videoPlayerDiv = document.querySelector('.video-player');
      if (videoPlayerDiv?.requestFullscreen) {
        videoPlayerDiv.requestFullscreen();
      }
    }
  }

  changeResolution(index: number) {
    this.resolutionChanged.emit(index);
    return false;
  }

  get iconPlaying() {
    return this.videoEnded
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

  ariaLabel() {
    return this.label;
  }
}

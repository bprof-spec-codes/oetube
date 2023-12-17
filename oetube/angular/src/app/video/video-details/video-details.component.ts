import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { PlaylistService, VideoService } from '@proxy/application';
import { VideoDto } from '@proxy/application/dtos/videos';
import { LazyTabItem } from 'src/app/lazy-tab-panel/lazy-tab-panel.component';
import { ActivatedRoute, Router } from '@angular/router';
import { PlaylistDto } from '@proxy/application/dtos/playlists';
import { VideoPlayerComponent } from '../video-player/video-player.component';
import { VideoTimeService } from 'src/app/services/video/video-time.service';
import { VideoPlayerService } from 'src/app/services/video/video-player.service';
@Component({
  selector: 'app-video-details',
  templateUrl: './video-details.component.html',
  styleUrls: ['./video-details.component.scss'],
})
export class VideoDetailsComponent {
  inputItems: LazyTabItem[] = [
    {
      key: 'watch',
      title: 'Watch',
      authRequired: false,
      onlyCreator: false,
      isLoaded: true,
      visible: true,
    },
    {
      key: 'edit',
      title: 'Edit',
      authRequired: true,
      onlyCreator: true,
      isLoaded: false,
      visible: true,
    },
  ];

  model: VideoDto;
  playlist: PlaylistDto;
  getRoute: Function;
  getMethod: Function;
  @ViewChild(VideoPlayerComponent) player: VideoPlayerComponent;

  pause() {
    this.player?.wrapper?.videoPlayerService.pause();
  }
  reset() {
    if (this.player?.wrapper) {
      this.player.wrapper.pause();
      this.player.wrapper.videoPlayerService.pause();
      this.player.wrapper.videoTimeService.setCurrentTime(0);
      this.player.wrapper.videoTimeService.setVideoProgress(0);
      this.player.wrapper.videoPlayerService.setVideoEnded(false);
    }
  }

  constructor(
    private videoService: VideoService,
    private changeDetector: ChangeDetectorRef,
    private playlistService: PlaylistService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.route.paramMap.subscribe(p => {
      const id = p.get('id');
      this.videoService.get(id).subscribe({
        next: v => {
          this.model = v;
          const playlistId = p.get('playlistId');
          if (playlistId) {
            this.playlistService.get(playlistId).subscribe({
              next:(p)=>{
                this.playlist = p;
                if(!this.playlist.items.includes(id)){
                  router.navigate(['video',id])
                  this.playlist=undefined
                }
                else{
                  this.getMethod = args => this.playlistService.getVideos(this.playlist.id, args);
                  this.getRoute = id => ['/video', id, this.playlist.id];
                  this.reset();
                  this.changeDetector.detectChanges();
                }
             
              },
              error:(e)=>{
                router.navigate(['video',id])
              }
            })
          } else {
            this.getMethod = args => this.videoService.getList(args);
            this.getRoute = id => ['/video', id];
            this.reset();
            this.changeDetector.detectChanges();
          }
        },
        error: e => {
          router.navigate(['/video']);
        },
      });
    });
  }
  onSelectedItemChange(e) {
    if (e.key == 'edit') {
      this.pause();
    }
  }
  onDeleted() {
    this.router.navigate(['video']);
  }
}

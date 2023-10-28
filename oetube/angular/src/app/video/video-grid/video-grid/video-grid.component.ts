import {Input, Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import {PagedResultDto,PagedResultRequestDto} from '@abp/ng.core'
import { VideoListItemDto } from '@proxy/application/dtos/videos';
// TODO Placeholder type!!! Remove as soon as possible
export type Video = {
  Id: string;
  Name: string;
  Description: string;
  Duration: number;
  CreationTime: Date;
  CreatorId: string;
  IsDeleted?: boolean;
};

@Component({
  selector: 'app-video-grid',
  templateUrl: './video-grid.component.html',
  styleUrls: ['./video-grid.component.scss'],
})
export class VideoGridComponent implements OnInit {
  //TODO make this @input
  @Input() videos: VideoListItemDto[];

  ngOnInit(): void {
    ;
  }


}

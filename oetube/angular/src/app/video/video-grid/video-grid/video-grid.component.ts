import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';

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
  public videos$: Observable<Video[]>;

  ngOnInit(): void {
    //Observable from Create Method
    this.videos$ = of([
      {
        Id: '1',
        Name: 'Test video',
        Description: 'Placeholder description',
        Duration: 5000,
        CreationTime: new Date(),
        CreatorId: 'guid',
      } as Video,
      {
        Id: '2',
        Name: 'Placeholder video',
        Description: 'Something witty',
        Duration: 20345,
        CreationTime: new Date(),
        CreatorId: 'guid2',
      } as Video,
      {
        Id: '3',
        Name: 'Informative video',
        Description: 'Something witty',
        Duration: 30,
        CreationTime: new Date(),
        CreatorId: 'guid2',
      } as Video,
      {
        Id: '4',
        Name: 'Just another movie',
        Description: 'With a nice description',
        Duration: 20345,
        CreationTime: new Date(),
        CreatorId: 'guid2',
      } as Video,
      {
        Id: '5',
        Name: 'Just another video with a rather long title which should never fit on the users screen',
        Description: 'With a nice description',
        Duration: 20345,
        CreationTime: new Date(),
        CreatorId: 'guid2',
      } as Video,
    ]);
  }

  fromatVideoDuration(duration: number) {
    const date = new Date(null);
    date.setSeconds(duration);
    return date.toISOString().slice(11, 19);
  }
}

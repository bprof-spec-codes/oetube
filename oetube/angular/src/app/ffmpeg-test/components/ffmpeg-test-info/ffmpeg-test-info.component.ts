import { Component, Input } from '@angular/core';
import { FFInfo } from 'src/app/upload/services/FF.service';
@Component({
  selector: 'app-ffmpeg-test-info',
  templateUrl: './ffmpeg-test-info.component.html',
  styleUrls: ['./ffmpeg-test-info.component.scss']
})

export class FfmpegTestInfoComponent {
  @Input() info?:FFInfo

 
  getText():string{
    if(!this.info){ 
      return "Info is undefined."
    }
    return JSON.stringify(this.info,null,4)
  
  }
}

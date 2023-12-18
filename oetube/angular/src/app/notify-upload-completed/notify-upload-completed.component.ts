import { Component,OnDestroy } from '@angular/core';
import { SignalrService } from 'src/app/services/video/signalr.service';
import { Observable,Subscription } from 'rxjs';
import { VideoDto } from '@proxy/application/dtos/videos';
import { VideoService } from '@proxy/application';
import {Router} from "@angular/router"
@Component({
  selector: 'app-notify-upload-completed',
  templateUrl: './notify-upload-completed.component.html',
  styleUrls: ['./notify-upload-completed.component.scss'],
})
export class NotifyUploadCompletedComponent implements OnDestroy {
  visible:boolean=false
  id:string
  message:string=""
  videoCompleted:Subscription
  constructor(private signalR:SignalrService){
    this.signalR.startConnection()
    this.signalR.onUploadCompleted()
    this.signalR.observe().subscribe(v=>{
      this.visible=true
      this.id=v.id
      this.message="'"+v.name+"' is avaliable!"

    })
  }
  testClick(){
    this.visible=true
  }
  ngOnDestroy(): void {
    this.signalR.stopConnection()
    this.videoCompleted.unsubscribe()
  }
}

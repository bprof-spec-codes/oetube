import { Component,ViewChild,ElementRef, OnInit,AfterViewInit } from '@angular/core';
import { VideoService } from '@proxy/application/services';
import * as abp from '@abp/ng.components';
import Hls from "hls.js";
import { SignalrService } from './services/signalr.service';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})

export class UploadComponent implements OnInit,AfterViewInit{
  @ViewChild("fileUploader",{static:true}) fileUploader:ElementRef<HTMLInputElement>
  constructor(private videoService:VideoService, private signalr:SignalrService){}
  @ViewChild("video",{static:true}) video:ElementRef<HTMLVideoElement>
    
  subscription:any;
  ngOnInit(): void {
      this.signalr.startConnection();
      this.signalr.subscribe()
      this.subscription=this.signalr.observe().subscribe((res:any)=>{
        this.loadSource(res)
    })
    }
    ngAfterViewInit(): void {
    const url="https://localhost:44348/products/video/b20e5819-226d-4a35-8227-3a0df6969502/480/list.m3u8"
    const videoElement= this.video.nativeElement;
    if(Hls.isSupported()){
      const hls=new Hls()
      hls.loadSource(url)
      hls.attachMedia(videoElement)
      hls.on(Hls.Events.MANIFEST_PARSED, ()=>{
        videoElement.play()
      })
    }
    else if(videoElement.canPlayType("application/vnd.apple.mpegurl")){
      videoElement.src=url
    }
  }
    loadSource(id:any){
      const url=environment.apis.default.url+`/api/app/video/src/${id}/480/list.m3u8`
      const videoElement= this.video.nativeElement;
    if(Hls.isSupported()){
      const hls=new Hls()
      hls.loadSource(url)
      hls.attachMedia(videoElement)
      hls.on(Hls.Events.MANIFEST_PARSED, ()=>{
        videoElement.play()
      })
    }
      else if(videoElement.canPlayType("application/vnd.apple.mpegurl")){
        videoElement.src=url
      }
    }
    onSubmit(){
      if(this.fileUploader.nativeElement.files.length<0){
        return
      }

      const file=this.fileUploader.nativeElement.files[0]
      const formData=new FormData()
      formData.append("content",file,file.name)
      this.videoService.uploadVideo(formData).subscribe(r=>{
      })
    }
  
  }  

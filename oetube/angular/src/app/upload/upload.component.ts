import { Component,ViewChild,ElementRef, OnInit,AfterViewInit } from '@angular/core';
import { VideoService } from '@proxy/application/video.service';
import { FFService } from './services/FF.service';
import { UploadTask } from '@proxy/infrastructure/video-file-manager';
import { VideoUploadStateDto } from '@proxy/application/dtos/videos';
import {firstValueFrom} from "rxjs"
import { type } from 'os';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})

export class UploadComponent implements OnInit,AfterViewInit{
  @ViewChild("fileUploader",{static:true}) fileUploader:ElementRef<HTMLInputElement>
  @ViewChild("video",{static:true}) video:ElementRef<HTMLVideoElement>

  progress:number
  log:any

  constructor(private videoService:VideoService, private ffService:FFService){
  }
  subscription:any;
  ngOnInit(): void {
    this.ffService.load();
    this.ffService.onProggress((progress)=>{
      this.progress=progress.ratio
    })
    this.ffService.onLogging((log)=>{
      this.log=log
    })
  }
    ngAfterViewInit(): void {
      
    }
    
    isTranscoding(){
      this.ffService.isTranscoding()
    }

    async onSubmit(){
      if(this.fileUploader.nativeElement.files.length<0){
        return
      }

      const file=this.fileUploader.nativeElement.files[0]
      const source=new FormData()
      source.append("content",file,file.name)
      
      const inputFileName="input."+file.name.split('.').pop()
      this.ffService.storeFile(file,inputFileName)
debugger;
      let state=await firstValueFrom(this.videoService.startUpload
        ({name:"Test",description:"test",content:source}))
      while(state.remainingTasks.length!=0){
        const format=state.outputFormat
        const nextTask=state.remainingTasks.pop()
        const outputFileName="output."+format
        
        const resizedFile=await this.ffService.transcode
          (inputFileName,outputFileName,nextTask.arguments)
          const resized=new FormData()
            resized.append("input",resizedFile,resizedFile.name)
        state=await firstValueFrom(this.videoService.continueUpload(state.id,resized))
      }

    
    }
  
  }  

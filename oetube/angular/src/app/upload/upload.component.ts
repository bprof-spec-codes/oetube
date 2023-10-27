import { Component, ViewChild,ContentChild,ElementRef, OnInit, AfterViewInit } from '@angular/core';
import { VideoService } from '@proxy/application/video.service';
import { FFService } from './services/FF.service';
import { StartVideoUploadDto, VideoUploadStateDto } from '@proxy/application/dtos/videos';
import { firstValueFrom } from 'rxjs';
import {FormControl, FormGroup} from '@angular/forms'
import { DxFileUploaderComponent } from 'devextreme-angular';
@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss'],
})
export class UploadComponent implements OnInit {
  @ViewChild('fileUploader', { static: true }) fileUploader:DxFileUploaderComponent;

  progress: number;
  log:any

  startVideoUpload:StartVideoUploadDto={
    name:"",
    description:"",
    content:undefined
  }
  submitButtonOptions={
    text:"Submit",
    useSubmitBehavior:true,
    type:"default"
  }

  constructor(private videoService: VideoService, private ffService: FFService) {}
  subscription: any;
  ngOnInit(): void {
    
    this.ffService.load();
    this.ffService.onProggress(progress => {
      this.progress = progress.ratio;
    });
    this.ffService.onLogging(log => {
      this.log = log;
      console.log(log);
    });
  }

  isTranscoding() {
    this.ffService.isTranscoding();
  }

  async onSubmit(event:Event) {
    if (this.fileUploader.value.length < 1) {
      return;
    }
    const file = this.fileUploader.value[0];
    const source = new FormData();
    source.append('content', file, file.name);
    this.startVideoUpload.content=source;
    const inputFileName = 'input.' + file.name.split('.').pop();

    this.ffService.storeFile(file, inputFileName);
    let state = await firstValueFrom(
    this.videoService.startUpload(this.startVideoUpload));
    while (state.remainingTasks.length != 0) {
      const format = state.outputFormat;
      const nextTask = state.remainingTasks.pop();
      const outputFileName = 'output.' + format;

      const resizedFile = await this.ffService.transcode(
        inputFileName,
        outputFileName,
        nextTask.arguments
      );
      const resized = new FormData();
      resized.append('input', resizedFile, resizedFile.name);
      state = await firstValueFrom(this.videoService.continueUpload(state.id, resized));
    }
  event.preventDefault();

  }
}

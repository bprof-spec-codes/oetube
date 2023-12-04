import {
  AfterViewInit,
  Component,
  ContentChild,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { StartVideoUploadDto, VideoUploadStateDto } from '@proxy/application/dtos/videos';

import { AccessType } from '@proxy/domain/entities/videos';
import { DxFileUploaderComponent } from 'devextreme-angular';
import { FFService } from './services/FF.service';
import { VideoService } from '@proxy/application/video.service';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss'],
})
export class UploadComponent implements OnInit {
  @ViewChild('fileUploader', { static: true }) fileUploader: DxFileUploaderComponent;

  progress: number;
  log: any;
  numberOfTasks: number;
  numberOfCompletedTasks: number;

  popupVisible = false;

  accessOptions = Object.values(AccessType).filter(x => typeof AccessType[x] != 'number');

  visibilityOptions = [
    { text: 'Public', value: 'Public' },
    { text: 'Private', value: 'Private' },
    { text: 'OE', value: 'OE' },
    { text: 'Custom Group', value: 'Custom Group' },
  ];

  selectedVisibility = this.visibilityOptions[0];

  startVideoUpload: StartVideoUploadDto = {
    name: '',
    description: '',
    access: AccessType.Public,
    content: undefined,
    accessGroups: []
  };
  submitButtonOptions = {
    text: 'Submit',
    useSubmitBehavior: true,
    type: 'default',
  };


  selectFileUploadButtonOptions = {
    type: 'default',
  };

  constructor(private videoService: VideoService, private ffService: FFService) {}
  subscription: any;
  ngOnInit(): void {
    this.ffService.load();
    this.ffService.onProgress(progress => {
      this.progress =
        (1 / this.numberOfTasks) * this.numberOfCompletedTasks +
        (1 / this.numberOfTasks) * progress.ratio;
    });
    this.ffService.onLogging(log => {
      this.log = log;
    });
  }
modelToJson(){
  return JSON.stringify(this.startVideoUpload,null,4)
}
  isTranscoding() {
    this.ffService.isTranscoding();
  }

  async onSubmit(event: Event) {
    if (this.fileUploader.value.length < 1) {
      return;
    }
    this.numberOfCompletedTasks = 0;
    this.progress = 0;
    const file = this.fileUploader.value[0];
    const source = new FormData();
    source.append('content', file, file.name);
    this.startVideoUpload.content = source;
    const inputFileName = 'input.' + file.name.split('.').pop();

    this.ffService.storeFile(file, inputFileName);
    let state = await firstValueFrom(this.videoService.startUpload(this.startVideoUpload));
    this.numberOfTasks = state.remainingTasks.length;

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
      this.numberOfCompletedTasks++;
      state = await firstValueFrom(this.videoService.continueUpload(state.id, {content: resized}));

    }
    event.preventDefault();
  }
}

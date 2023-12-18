import {
  Component,
  ViewChild,
  ContentChild,
  ElementRef,
  OnInit,
  AfterViewInit,
} from '@angular/core';
import { VideoService } from '@proxy/application/video.service';
import { StartVideoUploadDto, VideoUploadStateDto, WebAssemblyState } from '@proxy/application/dtos/videos';
import { delay, firstValueFrom } from 'rxjs';
import { FormControl, FormGroup } from '@angular/forms';
import {
  DxButtonComponent,
  DxFileUploaderComponent,
  DxRadioGroupComponent,
} from 'devextreme-angular';
import { AccessType } from '@proxy/domain/entities/videos';
import { GroupService } from '@proxy/application';
import { time } from 'console';
import { FFService } from 'src/app/services/video/FF.service';
import { ValidationStoreService } from 'src/app/services/validation-store.service';
import { VideoValidationDto } from '@proxy/application/dtos/validations';
@Component({
  selector: 'app-video-upload',
  templateUrl: './video-upload.component.html',
  styleUrls: ['./video-upload.component.scss'],
})
export class VideoUploadComponent implements OnInit {

  @ViewChild('fileUploader', { static: true }) fileUploader: DxFileUploaderComponent;
  @ViewChild('accessRadioGroup', { static: true }) accessRadioGroup: DxRadioGroupComponent;
  progress: number;
  log: any;
  uploadModel: StartVideoUploadDto = {
    name: '',
    description: '',
    access: AccessType.Public,
    content: undefined,
    accessGroups: [],
    webAssemblyState:WebAssemblyState.Enabled
  };
  numberOfTasks: number;
  numberOfCompletedTasks: number;

  showButtonOptions: Partial<DxButtonComponent & any>;
  accessTypeEnum = AccessType;
  accessOptions = Object.values(AccessType).filter(x => typeof AccessType[x] != 'number');
  webAssemblyEnabled:boolean=true
  submitButtonOptions = {
    text: 'Submit',
    useSubmitBehavior: true,
    type: 'normal',
  };

  selectFileUploadButtonOptions = {
    type: 'default',
  };
  isUploading=false
  isUploadingCompleted=false
  val:VideoValidationDto
  constructor(private videoService: VideoService, private ffService: FFService, validationStore:ValidationStoreService) {
    this.val=validationStore.validations.video
  }
  subscription: any;



 async ngOnInit() {
    try{
      await this.ffService.load();
      this.ffService.onProgress(progress => {
        this.progress =
          (1 / this.numberOfTasks) * this.numberOfCompletedTasks +
          (1 / this.numberOfTasks) * progress.ratio;
      });
      this.ffService.onLogging(log => {
        this.log = log;
      });
    }catch(e){
      console.log(e)
      this.webAssemblyEnabled=false
      this.uploadModel.webAssemblyState=WebAssemblyState.Disabled
    }

  }


  modelToJson() {
    return JSON.stringify(this.uploadModel, null, 4);
  }
  isTranscoding() {
    this.ffService.isTranscoding();
  }
  refresh(){
    window.location.reload()
  }
  colors:string[]=[
    "#1D294D",
    "#00449F",
    "#3B6BF9"
  ]
  createTaskIntervals(){
    if(this.numberOfTasks<1) return
    let range=100/this.numberOfTasks
    for (let i = 0; i < this.numberOfTasks; i++) {
      this.taskIntervals.push({min:i*range,max:(i+1)*range,color:this.colors[(this.colors.length-this.numberOfTasks)+i]})
    }
  }
 
  taskIntervals:{min:number,max:number,color:string}[]=[]
  hasTask:boolean=false
  async onSubmit(event: Event) {

    if (this.fileUploader.value.length < 1) {
      return;
    }
    this.numberOfCompletedTasks = 0;
    this.progress = 0;
    const file = this.fileUploader.value[0];
    const source = new FormData();
    source.append('content', file, file.name);
    this.uploadModel.content = source;
    const inputFileName = 'input.' + file.name.split('.').pop();

    if(this.webAssemblyEnabled){
      this.ffService.storeFile(file, inputFileName);
    }
    let state = await firstValueFrom(this.videoService.startUpload(this.uploadModel));
    this.isUploading=true
    this.numberOfTasks = state.remainingTasks.length;
    this.createTaskIntervals()
    this.hasTask=this.taskIntervals.length>0
    if(this.webAssemblyEnabled){
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
        resized.append('content', resizedFile, resizedFile.name);
        this.numberOfCompletedTasks++;
        state = await firstValueFrom(
          this.videoService.continueUpload(state.id, { content: resized })
        );
      }
    }
    
    this.isUploadingCompleted=true
    event.preventDefault();
  }
}

import { Component, OnInit,OnDestroy,ViewChild,ElementRef } from '@angular/core';
import { TranscoderResource, TranscoderService } from '../upload/services/Transcoder.service';
import { } from "devextreme-angular"
import { ValueChangedEvent } from 'devextreme/ui/calendar';
@Component({
  selector: 'app-ffmpeg-test',
  templateUrl: './ffmpeg-test.component.html',
  styleUrls: ['./ffmpeg-test.component.scss']
})



export class FfmpegTestComponent implements OnInit,OnDestroy {
  constructor(private transcoder:TranscoderService){
  }
  
  @ViewChild("inputDisplay",{static:true}) 
  inputDisplay:ElementRef<HTMLVideoElement>
  @ViewChild("outputDisplay",{static:true}) 
  outputDisplay:ElementRef<HTMLVideoElement>

  input?:TranscoderResource
  output?:TranscoderResource
  
  outputFormats:Array<string>=["mp4","webm","avi","mov"]
  outputFileName:string="output.mp4"
  
  command:string=""

  progress:number
  message:string
  
  async ngOnInit(): Promise<void> {
      this.transcoder.load()
      this.transcoder.onLogging((message)=>{
        this.message=message.message
      })
      this.transcoder.onProggress((progress)=>{
        this.progress=progress.ratio
      })
  }


  getCommand():string{
    if(this.input){
      return `-i ${this.input.fileName} ${this.command} ${this.outputFileName}`
    }
  }
  getExtension(fileName:string):string{
    return fileName.split('.')[1]
  }
  async onFileChanged(event:ValueChangedEvent){
    const file=(event.value[0] as File)
    const inputName="input."+this.getExtension(file.name)
    this.input=await this.transcoder.setResource(inputName,file)
    console.log(this.input)
    this.inputDisplay.nativeElement.src=this.input.objectUrl

  }
  onFormatChanged(event:ValueChangedEvent){
    this.outputFileName="output."+event.value
  }

async transcode(){
  this.output=await this.transcoder.transcode(this.input.fileName,this.outputFileName,this.command)
  console.log(this.output)
  this.outputDisplay.nativeElement.src=this.output.objectUrl
}
  ngOnDestroy(): void {
    this.transcoder.exit()
  }
  
}

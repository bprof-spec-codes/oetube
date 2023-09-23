import { Component, OnInit,OnDestroy,ViewChild,ElementRef } from '@angular/core';
import { FFInfo, FFService } from 'src/app/upload/services/FF.service';
import { ValueChangedEvent } from 'devextreme/ui/calendar';
@Component({
  selector: 'app-ffmpeg-test',
  templateUrl: './ffmpeg-test.component.html',
  styleUrls: ['./ffmpeg-test.component.scss']
})



export class FfmpegTestComponent implements OnInit,OnDestroy {
  constructor(private ff:FFService){
  }
  
  inputFileName:string
  @ViewChild("inputPlayer",{static:true}) 
  inputPlayer:ElementRef<HTMLVideoElement>
  inputInfo:FFInfo
  outputFileName:string="output.mp4"
  @ViewChild("outputPlayer",{static:true}) 
  outputPlayer:ElementRef<HTMLVideoElement>
  outputInfo:FFInfo
  transcoding:Promise<File>

  
  arguments:string=""

  progress?:number
  log?:{type:string, message:string}
  
  async ngOnInit(): Promise<void> {
      this.ff.load()
      this.ff.onLogging((message)=>{
        this.log=message
      })
      this.ff.onProggress((progress)=>{
        this.progress=progress.ratio
      })
  }
  

  getCommand():string{
    return this.ff.getCommand(this.inputFileName,this.outputFileName,this.arguments)
  }
  getExtension(fileName:string):string{
    return fileName.split('.')[1]
  }
 
  private resetVideo(player:ElementRef<HTMLVideoElement>,info:FFInfo)
  {
    URL.revokeObjectURL(player.nativeElement.src)
    player.nativeElement.src=""
    info=undefined
  }

  async onFileChanged(event:ValueChangedEvent){
    this.resetVideo(this.inputPlayer,this.inputInfo)
    this.inputFileName=""
    let file=(event.value[0] as File)
    if(file){
      this.inputFileName="input."+this.getExtension(file.name)
      file=await this.ff.storeFile(file,this.inputFileName)
      this.inputPlayer.nativeElement.src=URL.createObjectURL(file)
      this.inputInfo=await this.ff.getFileInfo(this.inputFileName)
      console.log(this.inputInfo)
    }
  }
  isTranscoding():boolean{
  return this.ff.isTranscoding()
}

async transcode(){
  this.resetVideo(this.outputPlayer,this.outputInfo)
  const file=await this.ff.transcode(this.inputFileName,this.outputFileName,this.arguments)
  this.outputPlayer.nativeElement.src=URL.createObjectURL(file)
  this.outputInfo=await this.ff.getFileInfo(file.name)
}

  ngOnDestroy(): void {
    this.ff.terminate()
  }
  
}

import { Injectable } from '@angular/core';
import { FFmpeg, createFFmpeg, fetchFile,CreateFFmpegOptions,LogCallback,ProgressCallback} from '@ffmpeg/ffmpeg';
import * as mime from 'mime-wrapper';


export class TranscoderResource{
  readonly fileName:string
  readonly objectUrl?:string
  readonly mimeType?:string

  constructor(fileName:string,data:Uint8Array){
    this.fileName=fileName
    this.mimeType=mime.mimeTypes.getType(fileName).split("/")[0]+"/"+fileName.split(".")[1]
    this.objectUrl=URL.createObjectURL(new Blob([data.buffer],{type:this.mimeType}))
  }
  getExtension():string{
    return this.fileName.split('.')[1]
  }
}

@Injectable({
  providedIn: 'root'
})

export class TranscoderService {
  ffmpeg:FFmpeg
  private resources:Map<string,TranscoderResource>=new Map<string,TranscoderResource>()

  constructor() 
  { 
  }
  onLogging(log:LogCallback){
    this.ffmpeg.setLogger(log)
  }
  onProggress(progress:ProgressCallback){
    this.ffmpeg.setProgress(progress)
  }

  async load(options?:CreateFFmpegOptions){
    if(this.ffmpeg==undefined){
      this.ffmpeg=createFFmpeg(options)
    }
    if(!this.ffmpeg.isLoaded()){
      await this.ffmpeg.load()
    }
  }

  async setResource(fileName:string,data:string|Blob|File|Buffer|Uint8Array):Promise<TranscoderResource>{
    if(this.resources.has(fileName)){
      this.deleteResource(fileName)
    }
    let input:Uint8Array
    if(data instanceof Uint8Array){
        input=data
      }
      else{
        input=await fetchFile(data)
      }
    
    this.ffmpeg.FS("writeFile",fileName,input)
    const resource=new TranscoderResource(fileName,input)
    this.resources.set(fileName,resource)
    return resource
  }

  async transcode(inputFileName:string,outputFileName:string,command:string):Promise<TranscoderResource>{
    if(this.resources.has(inputFileName))
    {
      if(this.resources.has(outputFileName)){
        this.deleteResource(outputFileName)
      }
      await this.ffmpeg.run('-i',inputFileName,...command.split(' '),outputFileName)
      const data=this.ffmpeg.FS("readFile",outputFileName)
      const resource=new TranscoderResource(outputFileName,data)
      this.resources.set(outputFileName,resource)
      return resource
    }
  }

  getResource(fileName:string):TranscoderResource{
    return this.resources.get(fileName)
  }
  deleteResource(fileName:string){
    if(this.freeResoure(fileName)){
      this.resources.delete(fileName)
    }
  }
  private freeResoure(fileName:string):boolean{
    const resource=this.resources.get(fileName)
    if(resource){
    this.ffmpeg.FS("unlink",fileName)
    URL.revokeObjectURL(resource.objectUrl)
    return true
  }
  return false
  }

  listResources():Array<TranscoderResource>{
    return Array.from(this.resources.values())
  }

  clearResources(){
    this.resources.forEach(element => {
      this.freeResoure(element.fileName)
    });
    this.resources.clear()
  }
  exit(){
    this.clearResources()
    this.ffmpeg.exit()
  }
}


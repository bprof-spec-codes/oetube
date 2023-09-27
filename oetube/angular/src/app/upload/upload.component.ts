import { Component,ViewChild,ElementRef } from '@angular/core';
import { VideoService } from '@proxy/application/services';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})

export class UploadComponent{
  @ViewChild("fileUploader",{static:true}) fileUploader:ElementRef<HTMLInputElement>

  constructor(private videoService:VideoService){}



    onSubmit(){
      if(this.fileUploader.nativeElement.files.length<0){
        return
      }

      const file=this.fileUploader.nativeElement.files[0]
      const formData=new FormData()
      formData.append("content",file,file.name)
      this.videoService.uploadVideo(formData).subscribe(r=>{})
    }
  
  }  

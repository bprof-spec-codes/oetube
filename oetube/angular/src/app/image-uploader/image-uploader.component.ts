import { Component,OnInit,OnDestroy,ViewChild, Output,EventEmitter } from '@angular/core';
import { DxFileUploaderComponent } from 'devextreme-angular';

@Component({
  selector: 'app-image-uploader',
  templateUrl: './image-uploader.component.html',
  styleUrls: ['./image-uploader.component.scss']
})

export class ImageUploaderComponent implements OnInit,OnDestroy{
  @ViewChild('fileUploader', { static: true }) fileUploader:DxFileUploaderComponent;

  files:File[]=[]
  imageFile:File

  @Output() imageFileChanged?:EventEmitter<File>=new EventEmitter<File>()

  imageUrl?:string
  onValueChanged(e){
    if(this.files.length>0){
      this.imageFile=this.files[0]
      this.imageUrl=URL.createObjectURL(this.imageFile)
      this.imageFileChanged.emit(this.imageFile)
    }else{
      this.clear()
    }
  }
  clear(){
    this.imageFile=undefined
    this.imageUrl=undefined
    this.imageFileChanged.emit(null)
    URL.revokeObjectURL(this.imageUrl)
  }
ngOnInit(): void {
    ;
}
ngOnDestroy(): void {
    this.clear()
}

}

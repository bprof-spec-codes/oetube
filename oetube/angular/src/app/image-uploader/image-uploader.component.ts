import { Component,Input,AfterViewInit,OnDestroy,ViewChild, Output,EventEmitter } from '@angular/core';
import { DxFileUploaderComponent } from 'devextreme-angular';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-image-uploader',
  templateUrl: './image-uploader.component.html',
  styleUrls: ['./image-uploader.component.scss']
})

export class ImageUploaderComponent implements OnDestroy{
  @ViewChild('fileUploader', { static: true }) fileUploader:DxFileUploaderComponent;

  @Input() height=220
  files:File[]=[]
  @Input() name:string
  @Input() value:FormData
  @Output() valueChange?:EventEmitter<FormData>=new EventEmitter<FormData>()

  private _defaultImgUrl:string
  @Input() set defaultImgUrl(value:string){
    this._defaultImgUrl=value
    if(!this.imageUrl){
      this.imageUrl=value
    }
  }
  imageUrl?:string

  
  onValueChange(e){
    this.clear()
    if(this.files.length>0){
      this.value=new FormData()
      this.value.append(this.name,this.files[0],this.files[0].name)
      this.imageUrl=URL.createObjectURL(this.files[0])
      this.valueChange.emit(this.value)
    }else{
      this.imageUrl=this._defaultImgUrl
      this.valueChange.emit(null)
    }
  }
  clear(){
    if(this.imageUrl!=this._defaultImgUrl){
      URL.revokeObjectURL(this.imageUrl)
    }
  }
ngOnDestroy(): void {
    this.clear()
    URL.revokeObjectURL(this._defaultImgUrl)
    
}

}

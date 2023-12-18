import { Component, Input,EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { VideoService } from '@proxy/application';
import { VideoValidationDto } from '@proxy/application/dtos/validations';
import { UpdateVideoDto, VideoDto } from '@proxy/application/dtos/videos';
import { AccessType } from '@proxy/domain/entities/videos';
import { ValidationStoreService } from 'src/app/services/validation-store.service';

@Component({
  selector: 'app-video-update',
  templateUrl: './video-update.component.html',
  styleUrls: ['./video-update.component.scss'],
})
export class VideoUpdateComponent {
  _inputModel:VideoDto
  @Input() set inputModel(v: VideoDto) {
    this._inputModel=v
    this.model = {
      access: v.access,
      accessGroups: v.accessGroups,
      description: v.description,
      name: v.name,
      indexImage:null
    };
  }
  get inputModel(){
    return this._inputModel
  }
  accessTypeEnum = AccessType;
  accessOptions = Object.values(AccessType).filter(x => typeof AccessType[x] != 'number');
  submitButtonOptions = {
    text: 'Submit',
    useSubmitBehavior: true,
    type: 'normal',
  };

  model: UpdateVideoDto;
  val:VideoValidationDto
  constructor(private service:VideoService,private router:Router,validationStore:ValidationStoreService){
    this.val=validationStore.validations.video
  }
  delete(){
      this.service.delete(this.inputModel.id).subscribe(r=>{
        this.router.navigate(["video"])
      })
  }
  onSubmit(e){
    this.service.update(this.inputModel.id,this.model).subscribe(r=>{
      window.location.reload()
    })
  }
}

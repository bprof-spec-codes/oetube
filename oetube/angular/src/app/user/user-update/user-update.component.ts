import { Component, Input, Output,EventEmitter } from '@angular/core';
import { OeTubeUserService } from '@proxy/application';
import { UpdateUserDto, UserDto } from '@proxy/application/dtos/oe-tube-users';
import { DxButtonComponent } from 'devextreme-angular';

@Component({
  selector: 'app-user-update',
  templateUrl: './user-update.component.html',
  styleUrls: ['./user-update.component.scss']
})
export class UserUpdateComponent {

  _inputModel:UserDto
  @Input() set inputModel(v:UserDto){
    this._inputModel=v
    this.model={image:null,name:v.name,aboutMe:v.aboutMe}
    this.defaultImageUrl=v.image
  }
  @Input() height:string
  defaultImageUrl:string
  model:UpdateUserDto
  @Output() submitted=new EventEmitter<UserDto>()

  constructor(private service:OeTubeUserService){
  }
  onSubmit($event){
    this.service.update(this._inputModel.id,this.model).subscribe({
      error:()=>console.log("error"),
      next:(r)=>this.submitted.emit(r)
    })
  }
  @Input() submitButtonOptions: Partial<DxButtonComponent> = {
    useSubmitBehavior: true,
    text: 'Update',
  };

}

import { SETTING_MANAGEMENT_VISIBLE_PROVIDERS } from '@abp/ng.setting-management/config';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { GroupService,OeTubeUserService } from '@proxy/application';
import { CreateUpdateGroupDto, GroupQueryDto } from '@proxy/application/dtos/groups';
import { ValueChangedEvent } from 'devextreme/ui/calendar';

@Component({
  selector: 'app-group-create',
  templateUrl: './group-create.component.html',
  styleUrls: ['./group-create.component.scss']
})
export class GroupCreateComponent implements OnInit, OnDestroy {
  submitButtonOptions={
    text:"Submit",
    useSubmitBehavior:true
  }
  imageFile:File

  model:CreateUpdateGroupDto={
    name:"",
    description:"",
    emailDomains:[],
    members:new Array<string>(),
    image:null
  }
  

  constructor(public groupService:GroupService,public userService:OeTubeUserService) {
  }


  ngOnInit(): void {
    ;
  }
  ngOnDestroy(): void {
    ;
  }
  onImageChanged(event:File){
    this.imageFile=event
    console.log( this.imageFile)

  }

  async onSubmit(event:Event){
    if(this.imageFile!=undefined){
      this.model.image=new FormData()
      this.model.image.append('image', this.imageFile, this.imageFile.name);
    }
    this.model.members=[]
    this.groupService.create(this.model).subscribe()
  }
  modelToJson(){
  return JSON.stringify(this.model,null,4)
  }
}

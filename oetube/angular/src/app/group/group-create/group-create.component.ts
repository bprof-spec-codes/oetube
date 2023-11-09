import { Component, OnInit } from '@angular/core';
import { GroupService,OeTubeUserService } from '@proxy/application';
import { PaginationDto } from '@proxy/application/dtos';
import { CreateUpdateGroupDto, GroupQueryDto } from '@proxy/application/dtos/groups';
import { UserListItemDto, UserQueryDto } from '@proxy/application/dtos/oe-tube-users';
import { VideoQueryDto } from '@proxy/application/dtos/videos';

@Component({
  selector: 'app-group-create',
  templateUrl: './group-create.component.html',
  styleUrls: ['./group-create.component.scss']
})
export class GroupCreateComponent implements OnInit {
  submitButtonOptions={
    text:"Submit",
    useSubmitBehavior:true
  }
  
  model:CreateUpdateGroupDto={
    name:"",
    description:"",
    emailDomains:["string"],
    members:[],
  }

  newEmailDomain:string="test"
  selectedUsers:Array<UserListItemDto>

  constructor(public groupService:GroupService,public userService:OeTubeUserService) {
  }


  ngOnInit(): void {
    ;
  }

  buttonDisabled(){
    return this.newEmailDomain==""||this.model.emailDomains.includes(this.newEmailDomain)
  }
  addNewEmailDomain(){
    this.model.emailDomains.push(this.newEmailDomain)
  }

  async onSubmit(event:Event){
  }
}

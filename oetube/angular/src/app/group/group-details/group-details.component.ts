import { Component, OnInit } from '@angular/core';
import {ActivatedRoute,Router } from '@angular/router'
import { Title } from '@angular/platform-browser';
import { GroupDto } from '@proxy/application/dtos/groups';
import { GroupService } from '@proxy/application';
import { CurrentUser, CurrentUserService } from 'src/app/services/current-user/current-user.service';
import { UserListItemDto } from '@proxy/application/dtos/oe-tube-users';
import { GroupItem } from 'devextreme/ui/form';
@Component({
  selector: 'app-group-details',
  templateUrl: './group-details.component.html',
  styleUrls: ['./group-details.component.scss']
})
export class GroupDetailsComponent implements OnInit {

  id:string
  model:GroupDto
  currentUser:CurrentUser
  selectedDatas:UserListItemDto[]
  selectedIndex:number
  constructor(private groupService:GroupService,private activatedRoute:ActivatedRoute,private title:Title,currentUserService:CurrentUserService,private router:Router){
    this.currentUser=currentUserService.get()
  }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(v=>{
      this.id=v.id
    })
    if(this.id==undefined){
      return
    }
    this.groupService.get(this.id).subscribe(r=>{
      this.model=r
      this.groupService.getExplicitMembersByIdAndInput(this.model.id,{pagination:{take:2<<32,skip:0}}).subscribe(r=>{
        this.selectedDatas=r.items
      })
    })
  }
  onDeleted(){
    this.router.navigate(["/group"])
  }
  onSubmitted(v:GroupDto){
    console.log(v)
    this.selectedIndex=0
    this.model=v
  }

}

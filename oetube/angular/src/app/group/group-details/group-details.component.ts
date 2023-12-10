import { Component, OnInit } from '@angular/core';
import {ActivatedRoute,Router } from '@angular/router'
import { Title } from '@angular/platform-browser';
import { GroupDto } from '@proxy/application/dtos/groups';
import { GroupService } from '@proxy/application';
import { UserListItemDto } from '@proxy/application/dtos/oe-tube-users';
import { GroupItem } from 'devextreme/ui/form';
import { CurrentUser, CurrentUserService } from 'src/app/auth/current-user/current-user.service';
import { LazyTabItem } from 'src/app/lazy-tab-panel/lazy-tab-panel.component';
import { AppTemplate } from 'src/app/template-ref-collection/template-ref-collection.component';
@Component({
  selector: 'app-group-details',
  templateUrl: './group-details.component.html',
  styleUrls: ['./group-details.component.scss']
})
export class GroupDetailsComponent implements OnInit {


  inputItems:LazyTabItem[]=[
    {key:"members",title:"Members",authRequired:false,onlyCreator:false,isLoaded:true,visible:true},
    {key:"edit",title:"Edit",authRequired:true,onlyCreator:true,isLoaded:false,visible:false}
  ]

  id:string
  model:GroupDto
  currentUser:CurrentUser
  selectedIndex:number

  constructor(private groupService:GroupService,private activatedRoute:ActivatedRoute,private title:Title,currentUserService:CurrentUserService,private router:Router){
    this.currentUser=currentUserService.getCurrentUser()
  }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(v=>{
      this.id=v.id
      this.groupService.get(this.id).subscribe(r=>{
        this.model=r
      })

    })
    if(this.id==undefined){
      return
    }
  }
  onDeleted(){
    this.router.navigate(["/group"])
  }
  onSubmitted(v:GroupDto){
    this.selectedIndex=0
    this.model=v
  }

}

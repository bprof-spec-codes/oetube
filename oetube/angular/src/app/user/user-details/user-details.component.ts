import { Component, OnInit } from '@angular/core';
import { OeTubeUserService } from '@proxy/application';
import { LazyTabItem } from 'src/app/lazy-tab-panel/lazy-tab-panel.component';
import {ActivatedRoute,ParamMap} from '@angular/router'
import { UserDto } from '@proxy/application/dtos/oe-tube-users';
@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.scss']
})
export class UserDetailsComponent implements OnInit {  inputItems:LazyTabItem[]=[
  {key:"details",title:"Details",authRequired:false,onlyCreator:false,isLoaded:true,visible:true},
  {key:"videos",title:"Videos",authRequired:false,onlyCreator:false,isLoaded:false,visible:true},
  {key:"playlists",title:"Playlists",authRequired:false,onlyCreator:false,isLoaded:false,visible:true},
  {key:"groups",title:"Groups",authRequired:false,onlyCreator:false,isLoaded:false,visible:true},
  {key:"edit",title:"Edit",authRequired:true,onlyCreator:true,isLoaded:false,visible:true},
]
  height:number=415
  model:UserDto
  id:string
  constructor(private userService:OeTubeUserService, private route:ActivatedRoute){
  }
ngOnInit(): void {
    this.route.paramMap.subscribe((params:ParamMap)=>{
      this.id=params.get('id')
      this.userService.get(this.id).subscribe(r=>{
        this.model=r
      })
    })
}

}

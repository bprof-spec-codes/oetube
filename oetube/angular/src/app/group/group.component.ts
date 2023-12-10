import { Component } from '@angular/core';
import { ActivatedRoute,Router } from '@angular/router';
import { GroupDto } from '@proxy/application/dtos/groups';
import { CurrentUser, CurrentUserService } from '../auth/current-user/current-user.service';
import { LazyTabItem } from '../lazy-tab-panel/lazy-tab-panel.component';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss'],
})
export class GroupComponent {



  inputItems:LazyTabItem[]=[
    {key:"explore",title:"Explore",authRequired:false,onlyCreator:false,isLoaded:true,visible:true},
    {key:"create",title:"Create",authRequired:true,onlyCreator:false,isLoaded:false,visible:false}
  ]

  constructor(private router:Router){
  }
  onSubmitted(e:GroupDto){
    this.router.navigate(['group/'+e.id])
  }
}


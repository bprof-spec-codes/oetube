import { Component } from '@angular/core';
import { ActivatedRoute,Router } from '@angular/router';
import { GroupDto } from '@proxy/application/dtos/groups';
import { CurrentUser, CurrentUserService } from '../services/current-user/current-user.service';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss'],
})
export class GroupComponent {

  currentUser:CurrentUser
  constructor(currentUserService:CurrentUserService,private router:Router){
    this.currentUser=currentUserService.getCurrentUser()
  }
  onSubmitted(e:GroupDto){
    this.router.navigate(['group/'+e.id])
  }
}


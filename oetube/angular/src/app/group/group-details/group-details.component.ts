import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
  styleUrls: ['./group-details.component.scss'],
})
export class GroupDetailsComponent {
  inputItems: LazyTabItem[] = [
    {
      key: 'members',
      title: 'Members',
      authRequired: false,
      onlyCreator: false,
      isLoaded: true,
      visible: true,
    },
    {
      key: 'edit',
      title: 'Edit',
      authRequired: true,
      onlyCreator: true,
      isLoaded: false,
      visible: false,
    },
  ];

  height: string = "60vh";
  id: string;
  model: GroupDto;
  currentUser: CurrentUser;
  selectedIndex: number;
  getMethod:Function
  constructor(
    private groupService: GroupService,
    private activatedRoute: ActivatedRoute,
    currentUserService: CurrentUserService,
    private router: Router
  ) {
    this.currentUser = currentUserService.getCurrentUser();
    this.activatedRoute.params.subscribe(v => {
      this.id = v.id;
      this.groupService.get(this.id).subscribe(r => {
        this.model = r;
      this.getMethod=(args)=>groupService.getGroupMembers(this.model.id,args)


      });
    });
  }

  onDeleted() {
    this.router.navigate(['/group']);
  }
  onSubmitted(v: GroupDto) {
    this.selectedIndex = 0;
    this.model = v;
  }
}

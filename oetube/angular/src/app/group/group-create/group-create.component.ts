import { SETTING_MANAGEMENT_VISIBLE_PROVIDERS } from '@abp/ng.setting-management/config';
import { Component, OnDestroy, OnInit, Input } from '@angular/core';
import { GroupService, OeTubeUserService } from '@proxy/application';
import { PaginationDto, QueryDto } from '@proxy/application/dtos';
import { CreateUpdateGroupDto, GroupDto, GroupQueryDto } from '@proxy/application/dtos/groups';
import { UserListItemDto } from '@proxy/application/dtos/oe-tube-users';
import { ValueChangedEvent } from 'devextreme/ui/calendar';
import { ScrollViewOptions } from 'src/app/scroll-view/scroll-view.component';
import { CurrentUserService } from 'src/app/services/current-user/current-user.service';

@Component({
  selector: 'app-group-create',
  templateUrl: './group-create.component.html',
  styleUrls: ['./group-create.component.scss'],
})
export class GroupCreateComponent implements OnInit {
  submitButtonOptions = {
    text: 'Submit',
    useSubmitBehavior: true,
  };

  @Input() contextId?: string;
  currentUserId?:string

  inputModel?: GroupDto;
  model: CreateUpdateGroupDto = {
    name: '',
    description: '',
    emailDomains: [],
    members: new Array<string>(),
    image: null,
  };
  defaultImgUrl: string;

  constructor(protected currentUserService: CurrentUserService, public groupService: GroupService) {
  this.currentUserId = this.currentUserService.get().id;
  }

  getInitialSelection = (id: string, args: QueryDto) =>
    this.groupService.getExplicitMembersByIdAndInput(id, args);

  ngOnInit(): void {
    if (this.contextId) {
      this.groupService.get(this.contextId).subscribe(r => {
        this.model = {
          name: r.name,
          description: r.description,
          emailDomains: r.emailDomains,
          members: r.members,
          image: null,
        };
        this.defaultImgUrl = r.image;
      });
    } else {
      this.groupService.getDefaultImage().subscribe(r => {
        this.defaultImgUrl = URL.createObjectURL(r);
        console.log(this.defaultImgUrl);
      });
    }
  }
 

  async onSubmit(event: Event) {
    this.model.members = [];
    this.groupService.create(this.model).subscribe();
  }
  modelToJson() {
    return JSON.stringify(this.model, null, 4);
  }
}

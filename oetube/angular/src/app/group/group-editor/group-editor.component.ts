import { SETTING_MANAGEMENT_VISIBLE_PROVIDERS } from '@abp/ng.setting-management/config';
import { Component, Output, OnDestroy, OnInit, Input, EventEmitter } from '@angular/core';
import { GroupService, OeTubeUserService } from '@proxy/application';
import { PaginationDto, QueryDto } from '@proxy/application/dtos';
import {
  CreateUpdateGroupDto,
  GroupDto,
  GroupListItemDto,
  GroupQueryDto,
} from '@proxy/application/dtos/groups';
import { UserListItemDto } from '@proxy/application/dtos/oe-tube-users';
import { DxButtonComponent } from 'devextreme-angular';
import { ValueChangedEvent } from 'devextreme/ui/calendar';
import { CurrentUserService } from 'src/app/services/current-user/current-user.service';
import { Router, provideRouter } from '@angular/router';
import { GroupModule } from '../group.module';
@Component({
  templateUrl: './group-editor.component.html',
  styleUrls: ['./group-editor.component.scss'],
})
export class GroupEditorComponent  {
  @Input() submitButtonOptions: Partial<DxButtonComponent>;
  @Input() title: string;
  inputModel: GroupDto;
  @Output() submitted=new EventEmitter<GroupDto>()
  model: CreateUpdateGroupDto;
  defaultImgUrl: string;

  constructor(protected groupService: GroupService) {}
 
  onSubmit(event: Event) {}
  modelToJson() {
    return JSON.stringify(this.model, null, 4);
  }
  protected delete(){
  }
}
@Component({
  selector: 'app-group-create',
  templateUrl: './group-editor.component.html',
  styleUrls: ['./group-editor.component.scss'],
})
export class GroupCreateComponent extends GroupEditorComponent implements OnInit {
  @Input() submitButtonOptions: Partial<DxButtonComponent> = {
    useSubmitBehavior: true,
    text: 'Submit',
  };
  @Input() title = 'Create a group';
  model = {
    name: '',
    description: '',
    emailDomains: [],
    members: [],
    image: null,
  };
  constructor(groupService: GroupService) {
    super(groupService);
  }
  ngOnInit(): void {
    this.groupService.getDefaultImage().subscribe(r => {
      this.defaultImgUrl = URL.createObjectURL(r);
    });
  }

  async onSubmit(event: Event) {
    this.groupService.create(this.model).subscribe({
      next: v => {
        this.submitted.emit(v)
      },
      error: e => {
        console.log(e);
      },
    });
  }
}
@Component({
  selector: 'app-group-update',
  templateUrl: './group-editor.component.html',
  styleUrls: ['./group-editor.component.scss'],
})
export class GroupUpdateComponent extends GroupEditorComponent implements OnInit {
  @Input() submitButtonOptions: Partial<DxButtonComponent> = {
    useSubmitBehavior: true,
    text: 'Update',
  };
  @Input() title: string = 'Edit your group';
  @Input() inputModel: GroupDto;
  @Input() selectedDatas: UserListItemDto[];
  
  

  @Output() deleted:EventEmitter<GroupDto>=new EventEmitter()
  constructor(groupService: GroupService) {
    super(groupService);
  }
  ngOnInit(): void {
    this.model = {
      name: this.inputModel.name,
      emailDomains: this.inputModel.emailDomains,
      members: this.inputModel.members,
      description: this.inputModel.description,
      image: null,
    };
    this.defaultImgUrl = this.inputModel.image;
  }

  async onSubmit(event: Event) {
    this.groupService.update(this.inputModel.id, this.model).subscribe({
      next: v => {
        this.submitted.emit(v)
      },
      error: e => {
        console.log(e);
      },
    });
  }

 public async delete(){
  this.groupService.delete(this.inputModel.id).subscribe({
    next:()=>{
      this.deleted.emit(this.inputModel)
    },
    error:(e)=>console.log(e)
  })
 }
}

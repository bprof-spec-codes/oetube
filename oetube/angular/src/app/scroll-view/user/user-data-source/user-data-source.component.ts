import { Component,ViewChild,forwardRef, Input } from '@angular/core';
import { GroupService, OeTubeUserService } from '@proxy/application';
import { UserListItemDto } from '@proxy/application/dtos/oe-tube-users';
import { DataSourceProviderDirective, ScrollViewDataSourceComponent } from '../../scroll-view-data-source/scroll-view-data-source.component';


@Component({
  selector: 'app-user-data-source',
  templateUrl: './user-data-source.component.html',
  styleUrls: ['./user-data-source.component.scss'],
  providers:[{provide:DataSourceProviderDirective,useExisting:forwardRef(()=>UserDataSourceComponent)}]
})
export class UserDataSourceComponent extends DataSourceProviderDirective<UserListItemDto>{
  @ViewChild(ScrollViewDataSourceComponent) _scrollViewDataSourceComponent:ScrollViewDataSourceComponent<UserListItemDto>


  get scrollViewDataSourceComponent(): ScrollViewDataSourceComponent<UserListItemDto> {
    return this._scrollViewDataSourceComponent
  }
  constructor(public service:OeTubeUserService){
    super()
  }

  getMethod=(args)=>this.service.getList(args)
}

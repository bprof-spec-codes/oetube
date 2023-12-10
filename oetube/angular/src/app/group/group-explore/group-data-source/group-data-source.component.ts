import { Component,ViewChild,forwardRef, Input } from '@angular/core';
import { GroupService } from '@proxy/application';
import { GroupListItemDto } from '@proxy/application/dtos/groups';
import { DataSourceProviderDirective, ScrollViewDataSourceComponent } from 'src/app/scroll-view/scroll-view-data-source/scroll-view-data-source.component';

@Component({
  selector: 'app-group-data-source',
  templateUrl: './group-data-source.component.html',
  styleUrls: ['./group-data-source.component.scss'],
  providers:[{provide:DataSourceProviderDirective,useExisting:forwardRef(()=>GroupDataSourceComponent)}]
})
export class GroupDataSourceComponent extends DataSourceProviderDirective<GroupListItemDto> {

  get scrollViewDataSourceComponent(): ScrollViewDataSourceComponent<GroupListItemDto> {
    return this._scrollViewDataSourceComponent
  }
  @ViewChild(ScrollViewDataSourceComponent) _scrollViewDataSourceComponent:ScrollViewDataSourceComponent<GroupListItemDto>
  constructor(public service:GroupService){
    super()
  }
  @Input() creatorId:string
  getMethod=(args)=>this.service.getList(args)

}

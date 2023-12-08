import { Component, forwardRef, Input, ViewChild } from '@angular/core';
import { GroupService, VideoService } from '@proxy/application';
import { GroupListItemDto } from '@proxy/application/dtos/groups';
import { DataSourceProviderDirective, ScrollViewDataSourceComponent } from '../../scroll-view-data-source/scroll-view-data-source.component';

@Component({
  selector: 'app-access-group-data-source',
  templateUrl: './access-group-data-source.component.html',
  styleUrls: ['./access-group-data-source.component.scss'],
  providers:[{provide:DataSourceProviderDirective,useExisting:forwardRef(()=>AccessGroupDataSourceComponent)}]
})
export class AccessGroupDataSourceComponent extends DataSourceProviderDirective<GroupListItemDto> {
  @ViewChild(ScrollViewDataSourceComponent) _scrollViewDataSourceComponent:ScrollViewDataSourceComponent<GroupListItemDto>
  @Input() videoId:string
  @Input() creatorId:string
  get scrollViewDataSourceComponent(): ScrollViewDataSourceComponent<GroupListItemDto>  {
    return this._scrollViewDataSourceComponent
  }

  constructor(public videoService:VideoService, public groupService:GroupService){
    super()
  }

  getMethod=(args)=>this.groupService.getList(args)
  getInitialSelectionMethod=(id,args)=>this.videoService.getAccessGroups(id,args)

}

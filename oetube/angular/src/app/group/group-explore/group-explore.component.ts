import { Component,Input,forwardRef,ViewChild, ChangeDetectorRef, AfterContentInit, AfterViewInit } from '@angular/core';
import { GroupService } from '@proxy/application';
import { PaginationDto } from '@proxy/application/dtos';
import { GroupListItemDto, GroupQueryDto } from '@proxy/application/dtos/groups';
import { Observable } from 'rxjs';
import { SearchItem } from 'src/app/scroll-view/drop-down-search/search-item';


@Component({
  selector: 'app-group-explore',
  templateUrl: './group-explore.component.html',
  styleUrls: ['./group-explore.component.scss'],
})
export class GroupExploreComponent{
  inputItems:SearchItem[]=[
    new SearchItem().init({key:"name",display:"Name"}),
    new SearchItem().init({key:"creationTime",display:"CreationTime"})
 ]



}


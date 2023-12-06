import {
  Component,
  Input,
} from '@angular/core';
import { OeTubeUserService } from '@proxy/application';
import { PaginationDto } from '@proxy/application/dtos';
import { UserListItemDto, UserQueryDto } from '@proxy/application/dtos/oe-tube-users';
import { Observable } from 'rxjs';
import { PaginationGridComponent } from '../pagination-grid.component';
import { Column } from 'devextreme/ui/data_grid';
import { Builder, Cloner } from 'src/app/base-types/builder';
import { CreationTimeColumnBuilder, DefaultFilterSetter, FilteredColumn} from '../columns';

@Component({
  selector: 'app-user-pagination-grid',
  templateUrl: '../pagination-grid.component.html',
  styleUrls: ['../pagination-grid.component.scss'],
})


export class UserPaginationGridComponent
  extends PaginationGridComponent<UserListItemDto>
{
  @Input() creationTime: FilteredColumn=new CreationTimeColumnBuilder().map({caption:"Registration"}).build()
  @Input() emailDomain:FilteredColumn=new EmailDomainColumnBuilder().build()
  
  constructor(protected userService: OeTubeUserService) {
    super();
  }

 buildColumns(): Column[] {
     return [this.thumbnail,this.id,this.name,this.emailDomain,this.creationTime]
 }
  getList(query: UserQueryDto): Observable<PaginationDto<UserListItemDto>> {
    return this.userService.getList(query);
  }
}

export class EmailDomainColumnBuilder extends Builder<FilteredColumn>{
  constructor(defaultAssigner?:Cloner) {
    super({
      visible:true,
    dataField:'emailDomain',
    caption:'Email Domain',
    dataType:'string',
    selectedFilterOperation:'contains',
    allowFiltering:true,
    allowSorting:true,
    filterOperations:[],
    filterSetter:new DefaultFilterSetter()
    },defaultAssigner)
  }

}

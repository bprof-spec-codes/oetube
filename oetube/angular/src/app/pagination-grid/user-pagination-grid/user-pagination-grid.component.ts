import { Component, TemplateRef, ViewChild } from '@angular/core';
import { OeTubeUserService } from '@proxy/application';
import { UserListItemDto, UserQueryDto } from '@proxy/application/dtos/oe-tube-users';
import { DataGridColumn, PaginationGridComponent } from '../pagination-grid.component';
import { LoadOptions } from 'devextreme/data';
import { Rest } from '@abp/ng.core/public-api';
import { HttpParameterCodec } from '@angular/common/http';
import { PaginationDto } from '@proxy/application/dtos';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user-pagination-grid',
  templateUrl: '../pagination-grid.component.html',
  styleUrls: ['./user-pagination-grid.component.scss']
})
export class UserPaginationGridComponent extends PaginationGridComponent<UserQueryDto,UserListItemDto> {
  constructor(public userService:OeTubeUserService){
    super()
    this.listInput={ 
      name:"",

    }
  }
  override getList(input: UserQueryDto, config?: 
    Partial<Partial<{ apiName: string; skipHandleError: boolean; skipAddingHeader: boolean; observe: Rest.Observe; httpParamEncoder?: HttpParameterCodec; }>>): Observable<PaginationDto<UserListItemDto>> {
    return this.userService.getList(input,config)
 }
  override initColumns(): DataGridColumn[] {
    return [
      {
        dataField:"thumbnailImageSource",
        caption:"Image"
      },
      {
        dataField:"name",
        caption:"Name"
      },
      {
        dataField:"emailDomain",
        caption:"Email Domain"
      },
      {
        dataField:"creationTime",
        caption:"Registration date"
      },
  ]  
  }
  override initInput(): UserQueryDto {
    return super.initInput()  
  }

  override beforeLoad(options: LoadOptions<any>, input: UserQueryDto): void {
   
 }
}

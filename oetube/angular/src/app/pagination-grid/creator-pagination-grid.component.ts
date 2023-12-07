import { Component, Input, OnInit, ViewChild,Output, EventEmitter, AfterViewInit} from '@angular/core';
import { PaginationDto, QueryDto } from '@proxy/application/dtos';
import { LoadOptions } from 'devextreme/data';
import { PaginationGridComponent } from './pagination-grid.component';
import { Column } from 'devextreme/ui/data_grid';
import { CurrentUserService } from 'src/app/services/current-user/current-user.service';
import { Builder, Cloner } from 'src/app/base-types/builder';


@Component({
    templateUrl: './pagination-grid.component.html',
    styleUrls: ['./pagination-grid.component.scss'],
    selector:'app-creator-pagination-grid'
  })
  export class CreatorPaginationGridComponent<TOutputListDto> extends PaginationGridComponent<TOutputListDto>{

    @Input() creator=new CreatorColumnBuilder().build()

    constructor( protected currentUserService:CurrentUserService){
       super()
      }
      isAuthenticated(){
        return this.currentUserService.getCurrentUser().isAuthenticated
      }
      creatorFilters=[
        {id:'All', value:()=>undefined},
        {id:'MySelf',value:()=>this.currentUserService.getCurrentUser()?.id}
      ]
      selectedCreatorFilter=this.creatorFilters[0]
      creatorFilterValue:string

      creatorFilterChanged(e){
        this.selectedCreatorFilter=e
        let value=e.value()
        if(value!=this.creatorFilterValue){
            this.dataSource.reload()
            this.creatorFilterValue=value
        }
      }
      loadOptionsToQuery(options: LoadOptions<any>): QueryDto {
         let query=super.loadOptionsToQuery(options)
         query['creatorId']=this.creatorFilterValue
         return query
      }
}

export class CreatorColumnBuilder extends Builder<Column>{
  constructor(defaultAssigner?:Cloner){
    super({
      visible:true,
      dataField: 'creator',
      dataType: 'object',
      cellTemplate:'creatorTemplate',
      allowFiltering:false,
      allowSorting:false,
      headerCellTemplate: 'creatorHeaderTemplate'
    })
  }
}

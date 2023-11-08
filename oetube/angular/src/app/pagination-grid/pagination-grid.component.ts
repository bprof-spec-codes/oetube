import { Rest } from '@abp/ng.core';
import { HttpParameterCodec } from '@angular/common/http';
import { Component, Input,Output,EventEmitter } from '@angular/core';
import { PaginationDto } from '@proxy/application/dtos';
import { LoadOptions } from 'devextreme/data';
import DataSource from 'devextreme/data/data_source';
import {lastValueFrom, Observable } from 'rxjs';


@Component({
  selector: 'app-pagination-grid',
  templateUrl: './pagination-grid.component.html',
  styleUrls: ['./pagination-grid.component.scss']
})

export class PaginationGridComponent<TInput extends QueryArgs,TOutput>{
  customDataSource: DataSource
  totalCount:number

  @Input() allowedPageSizes:[]
  @Input() getListInput:TInput
  @Input() getListProvider:GetListProvider<TInput,TOutput>
  @Output() beforeLoad:EventEmitter<BeforeLoadArgs<TInput>>=new EventEmitter<BeforeLoadArgs<TInput>>(true)

  constructor(){
    this.customDataSource=new DataSource<TOutput,string>(
      {
        load:async (options)=>{
          this.beforeLoad.emit({input:this.getListInput,options:options})
          return lastValueFrom(this.getListProvider.getList(this.getListInput))
              .then((response)=>{
                return {
                  data: response.items,
                  totalCount:response.totalCount
                }
              }).catch(()=>{
                throw 'loading error'
              })
        },
        totalCount: async ()=>{
            return this.totalCount
        },
      })
  }


  onPagingChanged(e:PagingChangedEventArgs){
    this.getListInput.itemPerPage=e.pageSize
    this.getListInput.page=e.pageIndex
  }
}
export interface BeforeLoadArgs<TInput>{
  options:LoadOptions
  input:TInput
}
export interface PagingChangedEventArgs{
  enabled?: boolean;
  pageIndex?: number;
  pageSize?: number;
}
export interface QueryArgs{
  itemPerPage?: number;
  page?: number;
  sorting?:string
}
export interface GetListProvider<TInput,TOutput>{
  getList(input:TInput,config?:Partial<Partial<{
    apiName: string;
    skipHandleError: boolean;
    skipAddingHeader: boolean;
    observe: Rest.Observe;
    httpParamEncoder?: HttpParameterCodec;
}>>):Observable<PaginationDto<TOutput>>
}
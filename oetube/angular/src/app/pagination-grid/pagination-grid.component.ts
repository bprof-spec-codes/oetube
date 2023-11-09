import { Injectable,Component, EventEmitter, OnInit,OnDestroy } from '@angular/core';
import {lastValueFrom, Observable } from 'rxjs';
import { PaginationDto } from '@proxy/application/dtos';
import { Rest } from '@abp/ng.core/public-api';
import { HttpParameterCodec } from '@angular/common/http';
import DataSource  from 'devextreme/data/data_source';
import { LoadOptions} from 'devextreme/data';
@Component({
  template:''
})
export abstract class PaginationGridComponent<TListProvider extends ListProvider<TListArgs,TOutputDto>,
TListArgs extends QueryArgs,TOutputDto> implements OnInit,OnDestroy
{
  listArgs:TListArgs={page:0,itemPerPage:0} as TListArgs
  dataSource:DataSource<TOutputDto,string>
  listProvider:TListProvider
  totalCount:number


  setPagination(options:LoadOptions):void{
    this.listArgs.itemPerPage=options.take
    this.listArgs.page=Math.floor(options.skip/options.take)
  }
  abstract beforeLoad(options:LoadOptions):void

  ngOnInit(): void {
      this.dataSource=new DataSource(
        {
          load:async (options)=>{
            this.setPagination(options)
            this.beforeLoad(options)
            return lastValueFrom(this.listProvider.getList(this.listArgs))
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
          paginate:true
        })
        
  }
  ngOnDestroy(): void {
      ;
  }
}

export interface ListProvider<TInput,TOutput>{
  getList(input:TInput,config?:Partial<Partial<{
    apiName: string;
    skipHandleError: boolean;
    skipAddingHeader: boolean;
    observe: Rest.Observe;
    httpParamEncoder?: HttpParameterCodec;
    }>>):Observable<PaginationDto<TOutput>>
}

export interface QueryArgs{
  itemPerPage?: number;
  page?: number;
  sorting?:string
}


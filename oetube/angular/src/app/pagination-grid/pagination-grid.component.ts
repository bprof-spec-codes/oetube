import { Component, Input, OnInit,OnDestroy } from '@angular/core';
import {lastValueFrom, Observable } from 'rxjs';
import { PaginationDto } from '@proxy/application/dtos';
import { Rest } from '@abp/ng.core/public-api';
import { HttpParameterCodec } from '@angular/common/http';
import DataSource  from 'devextreme/data/data_source';
import { LoadOptions } from 'devextreme/data';
@Component({
  template:''
})
export abstract class PaginationGridComponent<TListProvider extends ListProvider<TListArgs,TOutputDto>,
TListArgs extends QueryArgs,TOutputDto> implements OnInit,OnDestroy
{

  @Input() allowSelection:boolean
  @Input() selectedItems:Array<TOutputDto>=[]

  pageSizes:Array<number>=[10,20,50,100]
  listArgs:TListArgs={page:0,itemPerPage:this.pageSizes[0]} as TListArgs
  dataSource:DataSource<TOutputDto,string>
  listProvider:TListProvider
  allowedPageSizes:Array<number>

  setAllowedPageSizes(totalCount:number){
    this.allowedPageSizes=[this.pageSizes[0]]
    for (let index = 1; index < this.pageSizes.length; index++) {
      const element = this.pageSizes[index];
      if(element>totalCount){
        break;
      }
      this.allowedPageSizes.push(element)
    }
  }
  handlePagination(options:LoadOptions):void{
    if(options.take!=null&&options.take>0){
      this.listArgs.itemPerPage=options.take
      if(options.skip!=null&&options.skip>=0){
        this.listArgs.page=Math.floor(options.skip/options.take)
      }
    }
  }
  findFilterValue(items:Array<any>,operation:string,name:string){
    if(items?.length>=2){
      if(items[0]==name&&items[1]==operation){
        return items[2]
      }
      else{
        for (let index = 0; index <items.length; index++) {
          const element = items[index];
          if(element instanceof Array){
            const result=this.findFilterValue(element,operation,name)
            if(result!=null){
              return result
            }
          }
         }
      }
    }
    return null
  }

  handleSorting(args:{selector:string,desc:boolean}): void 
  {
    debugger
      this.listArgs.sorting=args.selector+(args.desc?" desc":" asc")
  }
  abstract handleFilter(options:LoadOptions):void
  
  ngOnInit(): void {
      this.dataSource=new DataSource(
        {
          load:async (options)=>{
            console.log(options)
            this.handlePagination(options)
            this.handleFilter(options)
            if(options.sort instanceof Array && options.sort.length>0){
              this.handleSorting(options.sort[0] as {selector:string,desc:boolean})
            }
            return lastValueFrom(this.listProvider.getList(this.listArgs))
                .then((response)=>{
                  this.setAllowedPageSizes(response.totalCount)
                  return {
                    data: response.items,
                    totalCount:response.totalCount
                  }
                }).catch(()=>{
                  throw 'loading error'
                })

          },
          paginate:true
        })
        
  }
  ngOnDestroy(): void {
      ;
  }
}
export interface SortArgs{
  name:string
  desc:boolean
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


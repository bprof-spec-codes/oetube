import { Component, Input, OnInit, OnDestroy, ViewChild,Output, EventEmitter} from '@angular/core';
import { lastValueFrom, Observable } from 'rxjs';
import { PaginationDto, QueryDto } from '@proxy/application/dtos';
import { Rest } from '@abp/ng.core/public-api';
import { HttpParameterCodec } from '@angular/common/http';
import DataSource from 'devextreme/data/data_source';
import { LoadOptions } from 'devextreme/data';
import { DxDataGridComponent } from 'devextreme-angular';
@Component({
  template: '',
})
export abstract class PaginationGridComponent<
  TListProvider extends ListProvider<TQueryArgs, TOutputDto>,
  TQueryArgs extends QueryDto,
  TOutputDto
> implements OnInit, OnDestroy
{
  defaultItemPerPage: number = 10;
  pageSizes: Array<number> = [this.defaultItemPerPage, 20, 50, 100];
  dataSource: DataSource<TOutputDto, string>;
  listProvider: TListProvider;
  allowedPageSizes: Array<number>;
  
  @ViewChild("dataGrid",{static:true}) dataGrid:DxDataGridComponent
  @Input() height:number
  @Input() width:number
  @Input() queryArgs: TQueryArgs={itemPerPage:this.defaultItemPerPage,page:0} as TQueryArgs
  @Input() allowSelection: boolean=true

  @Input() selectedItems:Array<string>
  initSelectedRowKeys(){
    this.dataGrid.selectedRowKeys=this.selectedItems.map(i=>{id:i})
  }
  @Output() selectedItemsChange:EventEmitter<Array<string>>=new EventEmitter<Array<string>>()
  subscribeSelectedRowKeyChange(){
    this.dataGrid.selectedRowKeysChange.subscribe((value:{id:string}[])=>{
      this.selectedItemsChange.emit(value.map(item=>item.id))
    })
  }
 


  setAllowedPageSizes(totalCount: number) {
    this.allowedPageSizes = [this.pageSizes[0]];
    for (let index = 1; index < this.pageSizes.length; index++) {
      const element = this.pageSizes[index];
      if (element > totalCount) {
        break;
      }
      this.allowedPageSizes.push(element);
    }
  }
  handleResponseData(data:PaginationDto<TOutputDto>){

  }
  handlePagination(options: LoadOptions): void {
    if (options.take == null && options.take > 0) {
      this.queryArgs.itemPerPage = options.take;
      if (options.skip != null && options.skip >= 0) {
        this.queryArgs.page = Math.floor(options.skip / options.take);
      }
    }
  }
  findFilterValue(items: Array<any>, operation: string, name: string) {
    if (items?.length >= 2) {
      if (items[0] == name && items[1] == operation) {
        return items[2];
      } else {
        for (let index = 0; index < items.length; index++) {
          const element = items[index];
          if (element instanceof Array) {
            const result = this.findFilterValue(element, operation, name);
            if (result != null) {
              return result;
            }
          }
        }
      }
    }
    return null;
  }

  handleSorting(args: { selector: string; desc: boolean }): void {
    this.queryArgs.sorting = args.selector + (args.desc ? ' desc' : ' asc');
  }
  abstract handleFilter(options: LoadOptions): void;

  ngOnInit(): void {
    this.dataSource = new DataSource({
      load: async options => {
        if(this.queryArgs.itemPerPage==null){
          this.queryArgs.itemPerPage=this.defaultItemPerPage
        }
        this.handlePagination(options);
        this.handleFilter(options);
        if (options.sort instanceof Array && options.sort.length > 0) {
          this.handleSorting(options.sort[0] as { selector: string; desc: boolean });
        }
        
        return lastValueFrom(this.listProvider.getList(this.queryArgs))
          .then(response => {

            this.setAllowedPageSizes(response.totalCount);
            this.handleResponseData(response)
            return {
              data: response.items,
              totalCount: response.totalCount,
            };
          })
          .catch(() => {
            throw 'loading error';
          });
      },
    
      key:["id"],
      paginate: true,
    });
    this.initSelectedRowKeys()
    this.subscribeSelectedRowKeyChange()
  }
  ngOnDestroy(): void {;}
}

export interface ListProvider<TInput, TOutput> {
  getList(
    input: TInput,
    config?: Partial<
      Partial<{
        apiName: string;
        skipHandleError: boolean;
        skipAddingHeader: boolean;
        observe: Rest.Observe;
        httpParamEncoder?: HttpParameterCodec;
      }>
    >
  ): Observable<PaginationDto<TOutput>>;
}


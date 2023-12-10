import { Component, Input, OnInit, ViewChild,Output, EventEmitter, AfterViewInit, Pipe, PipeTransform, ElementRef, TemplateRef} from '@angular/core';
import { filter, lastValueFrom, Observable } from 'rxjs';
import { PaginationDto, QueryDto } from '@proxy/application/dtos';
import DataSource from 'devextreme/data/data_source';
import { LoadOptions } from 'devextreme/data';
import { DxDataGridComponent } from 'devextreme-angular';
import {ConfigStateService} from '@abp/ng.core'
import { CreationTimeColumnBuilder, FilteredColumn, IdColumnBuilder, NameColumnBuilder, ThumbnailColumnBuilder} from './columns';
import  {CellClickEvent, Pager } from 'devextreme/ui/data_grid';
import { Column } from 'devextreme/ui/data_grid'
import DevExpress from 'devextreme';
import { Type } from '@angular/compiler';
import { Query } from 'devextreme/data/query';
import { NotifyInit, TwoWayBinding } from 'src/app/base-types/two-way-binding';
import { Builder, ObjectPropertyChanger } from 'src/app/base-types/builder';

@Component({
  templateUrl: './pagination-grid.component.html',
  styleUrls:['./pagination-grid.component.scss'],
  selector:'app-pagination-grid'
})
export class PaginationGridComponent<
  TOutputListDto,
> implements OnInit, NotifyInit
{
  initialized?: EventEmitter<any>
  dataSource: DataSource<TOutputListDto, string>
  @ViewChild(DxDataGridComponent, {static:true}) dxDataGrid:DxDataGridComponent
  defaultItemPerPage: number = 10;
  selectedPageSize:number=this.defaultItemPerPage
  pageSizes: Array<number> = [this.defaultItemPerPage, 20, 50, 100];
  
  @Input() preprocessResponse:(response:PaginationDto<TOutputListDto>)=>PaginationDto<TOutputListDto>
  @Input() id=new IdColumnBuilder().build()
  @Input() name=new NameColumnBuilder().build()
  @Input() thumbnail=new ThumbnailColumnBuilder().build()
  @Input() creationTime=new CreationTimeColumnBuilder().build()
  @Input() dataGridBuilder:Builder<DxDataGridComponent>

  @TwoWayBinding<PaginationGridComponent<TOutputListDto>>
  ((selector)=>selector.get('dxDataGrid').get('selectedRowKeys'),
  {
    convert:(v:{id:string}[])=>v.map(i=>i.id),
    convertBack:(v:{id:string}[])=>v.map(i=>{return {id:i}})
  })
  @Input() selectedKeys: Array<string>
  @Output() selectedKeysChange: EventEmitter<Array<string>> = new EventEmitter<Array<string>>()
  @Output() cellClicked:EventEmitter<CellClickEvent<TOutputListDto,string>>=new EventEmitter<CellClickEvent<TOutputListDto,string>>()

  protected propertyChanger:ObjectPropertyChanger<DxDataGridComponent>

  ngOnInit(): void {
    this.initDataGrid()
    this.propertyChanger=new ObjectPropertyChanger<DxDataGridComponent>(this.dxDataGrid)
    this.initialized.emit(this)
  }
 
  createDataSource() {
   return new DataSource<TOutputListDto,string>({
      load: async options => {
        console.log("load")
        let query=this.loadOptionsToQuery(options)
        return lastValueFrom(this.getList(query))
          .then(response => {
            if(this.preprocessResponse){
              response=this.preprocessResponse(response)
            }
            //this.refreshPaging(response.totalCount);
            return {
              data: response.items,
              totalCount: response.totalCount,
            };
          })
          .catch(() => {
            throw 'loading error';
          });
      },
      key: ["id"],
      paginate: true,
    });
  }
  log(data){
    console.log(data)
  }
  initDataGrid() {
    this.dataGridBuilder??=new DefaultDataGridBuilder(this.dxDataGrid)
    this.dataSource=this.createDataSource()
    this.dxDataGrid.dataSource=this.dataSource
    this.dxDataGrid.columns=this.buildColumns()
  }
  buildColumns():Column[]
  {
    return []
  }


  getList(query:QueryDto): Observable<PaginationDto<TOutputListDto>>{
    return undefined
  }


  refreshPaging(totalCount: number) {
    const allowedPageSizes=[]
    for (let index = 0; index < this.pageSizes.length; index++) {
      const element = this.pageSizes[index];
     
      allowedPageSizes.push(element);
      if (element>totalCount) {
        break;
      }
    }
    debugger
    let currentPageSize=this.dxDataGrid.paging.pageSize
    if(!allowedPageSizes.includes(currentPageSize))
    {
      currentPageSize=allowedPageSizes[allowedPageSizes.length-1]
    }
    this.propertyChanger.changeProperty('pager',{allowedPageSizes:allowedPageSizes})
    this.propertyChanger.changeProperty('paging',{pageSize:currentPageSize})
  }

  loadOptionsToQuery(options:LoadOptions):QueryDto{
    let query:QueryDto={
      pagination:{take:0,skip:0},
      sorting:""
    }
    if (options?.take > 0) {
      query.pagination.take = options.take;
      if (options?.skip >= 0) {
        query.pagination.skip =options.skip;
      }
    }
    if (options.sort instanceof Array && options.sort.length > 0) {
       let sorting=options.sort[0] as {selector:string, desc:boolean}
       if(sorting){
          query.sorting=sorting.selector + (sorting.desc ? ' desc' : ' asc');
       }
    }
    this.dxDataGrid.columns.forEach(c=>{
      let column=c as FilteredColumn
      if(column.filterSetter!=undefined){
        column.filterSetter.setFilterValue(options,query,column)
      }
    })
    return query
  }
}
export class DefaultDataGridBuilder extends Builder<DxDataGridComponent>{
  constructor(prototype:DxDataGridComponent){
    super(prototype)
    this.map({
      remoteOperations: true,
      cacheEnabled: true,
      showBorders: true,
      allowColumnResizing: true,
      columnAutoWidth: true,
      columnResizingMode: "nextColumn",
      showColumnHeaders: true,
      hoverStateEnabled: true,
      showRowLines:true,
      pager:{
        showInfo:true,
        showNavigationButtons: true,
        visible: true,
        showPageSizeSelector: true,
      },
      filterRow:{
        visible:true
      },
      selection:{
        allowSelectAll:true,
        mode: "multiple",
        showCheckBoxesMode:"always",
        selectAllMode: "page"
      },
      paging:{
        pageSize:10,
        pageIndex:0,
        enabled:true
      }
    })
  }
}


import { Component, Input, OnInit, OnDestroy, ViewChild,Output, EventEmitter} from '@angular/core';
import { lastValueFrom, Observable } from 'rxjs';
import { PaginationDto, QueryDto } from '@proxy/application/dtos';
import DataSource from 'devextreme/data/data_source';
import { LoadOptions } from 'devextreme/data';
import { DxDataGridComponent } from 'devextreme-angular';
import { DxiButtonComponent, DxoEditingComponent, DxoPagerComponent, DxoPagingComponent, DxoSelectionComponent } from 'devextreme-angular/ui/nested';
@Component({
  template: '',
})
export abstract class PaginationGridComponent<
  TQueryArgs extends QueryDto,
  TOutputDto,
  TOutputListDto,
  TUpdateDto
> implements OnInit, OnDestroy
{
  defaultItemPerPage: number = 10;
  pageSizes: Array<number> = [this.defaultItemPerPage, 20, 50, 100];
  dataSource: DataSource<TOutputListDto, string>;
  allowedPageSizes: Array<number>;
  allowDelete:boolean=true
  allowEdit:boolean=true

  @ViewChild("dxGrid",{static:true}) dxGrid:DxDataGridComponent
  @ViewChild("dxPager",{static:true}) dxPager:DxoPagerComponent
  @ViewChild("dxPaging",{static:true}) dxPaging:DxoPagingComponent
  @ViewChild("dxEditButton",{static:true}) dxEditButton:DxiButtonComponent 
  @ViewChild("dxDeleteButton",{static:true}) dxDeleteButton:DxiButtonComponent 
  @ViewChild("dxShowDetailsButton",{static:true}) dxShowDetailsButton:DxiButtonComponent 
  @ViewChild("dxGridSelection",{static:true}) dxGridSelection:DxoSelectionComponent
  @ViewChild("dxEditing",{static:true}) dxEditing:DxoEditingComponent
  
  @Input() height:number
  @Input() width:number
  @Input() queryArgs: TQueryArgs={itemPerPage:this.defaultItemPerPage,page:0} as TQueryArgs
  @Input() showId:boolean=true
  @Input() selectionMode: string="multiple"
  @Input() selectedItems:Array<string>
  @Input() updateModel:TUpdateDto
  @Output() updateModelChanged:EventEmitter<TUpdateDto>=new EventEmitter<TUpdateDto>()

  
  

  initDataGrid(){
    this.dxGrid.dataSource=this.dataSource
    this.dxGrid.remoteOperations=true
    this.dxGrid.cacheEnabled=true
    this.dxGrid.showBorders=true
    this.dxGrid.allowColumnResizing=true
    this.initPaging()
    this.initPager()
    this.initSelectedRowKeys()
    this.subscribeSelectedRowKeyChange()
  }
  initPaging(){

  }
  initPager(){
    this.dxPager.visible=true
    this.dxPager.allowedPageSizes=this.allowedPageSizes
    this.dxPager.displayMode="full"
    this.dxPager.showInfo=true
    this.dxPager.showNavigationButtons=true
    this.dxPager.showPageSizeSelector=true
  }
  initSelection(){
    this.dxGridSelection.showCheckBoxesMode="always"
    this.dxGridSelection.selectAllMode="page"

  }
  initSelectedRowKeys(){
    if(this.selectedItems){
      this.dxGrid.selectedRowKeys=this.selectedItems.map(i=>{id:i})
    }
  }
  @Output() selectedItemsChange:EventEmitter<Array<string>>=new EventEmitter<Array<string>>()
  subscribeSelectedRowKeyChange(){
    this.dxGrid.selectedRowKeysChange.subscribe((value:{id:string}[])=>{
      this.selectedItemsChange.emit(value.map(item=>item.id))
    })
  }
 
  abstract getList(): Observable<PaginationDto<TOutputListDto>>
  
  getDetails(key:string):Observable<TOutputDto>{
    return undefined
  }
  delete(key:string):Observable<any>{
    return undefined
  }

  mapOutputToUpdate(dto:TOutputDto):TUpdateDto{
    return undefined
  }


  update(model:TUpdateDto):Observable<TOutputDto>{
  return undefined   
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
  handleListData(data:PaginationDto<TOutputListDto>){
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
        
        return lastValueFrom(this.getList())
          .then(response => {

            this.setAllowedPageSizes(response.totalCount);
            this.handleListData(response)
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
      remove:async(key)=>{
        await lastValueFrom(this.delete(key))
      },
      update:async(key,values)=>{
        this.updateModel=this.mapOutputToUpdate(await lastValueFrom(this.update(this.updateModel)))
      }
    });
   
  }
  ngOnDestroy(): void {;}
}


import { Component,Input,Output,EventEmitter,forwardRef, Directive} from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import type { EntityDto } from '@abp/ng.core';
import { Filter } from '../drop-down-search/drop-down-search.component';
import { PaginationDto, QueryDto } from '@proxy/application/dtos';
import { Observable,lastValueFrom } from 'rxjs';
import { Query } from 'devextreme/data/query';
import { Pagination } from '@proxy/domain/repositories/query-args';


export type LoadType="refresh"|"loadNext"

@Directive({
selector:"[appDataSourceProvider]",
})
export abstract class DataSourceProviderDirective<TOutputListDto extends EntityDto<string> = EntityDto<string>>{
  abstract get scrollViewDataSourceComponent():ScrollViewDataSourceComponent<TOutputListDto>
}

@Component({
  selector: 'app-scroll-view-data-source',
  template: '',
  providers:[{provide:DataSourceProviderDirective,useExisting:forwardRef(()=>ScrollViewDataSourceComponent)}]
})
export class ScrollViewDataSourceComponent<TOutputListDto extends EntityDto<string> = EntityDto<string>> extends DataSourceProviderDirective<TOutputListDto> {

  get scrollViewDataSourceComponent(): ScrollViewDataSourceComponent<TOutputListDto> {
    return this    
  }
  protected dataSource: DataSource<TOutputListDto, string>;
  totalCount: number;
  
  cachedData: TOutputListDto[] = [];

  @Input() filteredKeys: string[] = [];
  @Output() filteredKeysChange:EventEmitter<string[]>=new EventEmitter()

  @Input() allowSelection:boolean

  @Input() selectionContextId:string
  @Input() getInitialSelectionMethod:(id:string,args:QueryDto)=>Observable<PaginationDto<TOutputListDto>>

  @Input() selectedDatas: TOutputListDto[] = [];
  @Output() selectedDatasChange:EventEmitter<TOutputListDto[]>=new EventEmitter<TOutputListDto[]>()
  

  @Input() getMethod:(args:QueryDto)=>Observable<PaginationDto<TOutputListDto>>

  @Input() set creatorId(v:string){
    this.query["creatorId"]=v
  }
  get creatorId(){
    return this.query["creatorId"]
  }


  @Input() set takePerLoad(v:number){
    if(!this.query.pagination){
      this.query.pagination={
        skip:0,
        take:v
      }
    }
  }
  get takePerLoad(){
    return this.query.pagination.take
  }

  private defaultPagination:Pagination={take:30,skip:0}
  
  _query: QueryDto={
    pagination:{
      take:this.defaultPagination.take,
      skip:this.defaultPagination.skip
    },
    sorting:""
  }
get query():QueryDto{
  return this._query
}
  @Input() set query(v:QueryDto){
    if(!v){
      v={pagination:{take:this.defaultPagination.take,skip:this.defaultPagination.skip},sorting:""}
    }
    if(!v.pagination){
      v.pagination={take:this.defaultPagination.take,skip:this.defaultPagination.skip}
    }
    this._query=v
  }
  loading:boolean=false
  @Output() beginLoad:EventEmitter<LoadType>=new EventEmitter<LoadType>()
  @Output() endLoad:EventEmitter<LoadType>=new EventEmitter<LoadType>()

 private async startLoading(loadFunction:()=>Promise<void>,loadType:LoadType){
  try{
    console.log("start loading:")
    console.log(this.query)
    console.log(loadType)
    console.log(this)
    if(this.loading) throw ("loading!")
    this.loading=true;
    this.beginLoad.emit(loadType)
    await loadFunction()
      this.loading=false
        this.endLoad.emit(loadType)
        console.log("load complete")
  }catch(error){
    console.log(error)
  }
}
  async refresh() {
    await this.startLoading( async ()=>{
   
      if (!this.getMethod) {
        return;
      }
      if (!this.dataSource) {
        this.dataSource = this.createDataSource();
      }
    this.clearCachedData()
     await this.dataSource.reload()
     
    },"refresh");
  }

  deselectItem(item:TOutputListDto){
    if(this.allowSelection){
      debugger
      const index=this.selectedDatas.indexOf(item)
      if(index!=-1){
        this.selectedDatas.splice(index,1)
            }     
           console.log(item)
        this.cachedData.unshift(item)
        this.selectedDatasChange.emit(this.selectedDatas)
      }
    }

  selectItem(item:TOutputListDto){
    if(this.allowSelection){
      const index=this.cachedData.indexOf(item)
      if(index!=-1){
        this.cachedData.splice(index,1);
        this.selectedDatas.push(item)
        this.selectedDatasChange.emit(this.selectedDatas)
      }

    }
  }

  async reload(){
    await this.startLoading(async ()=>{
      if (!this.getMethod) {
        return;
      }
      if (!this.dataSource) {
        this.dataSource = this.createDataSource();
      }
      this.clearDatas()
      if(this.allowSelection&&this.selectionContextId&&this.getInitialSelectionMethod){
          const response=await lastValueFrom(this.getInitialSelectionMethod(this.selectionContextId,{pagination:{skip:0,take:2<<32}}))
          this.selectedDatas.push(...response.items)
          this.selectedDatasChange.emit(this.selectedDatas)
      }
      else{
        this.clearSelection()
      }
      await this.dataSource.reload()
    },"refresh")
  }


  clearSelection(){
    if(this.allowSelection){
      this.cachedData.unshift(...this.selectedDatas);
      this.selectedDatas=[]
      this.selectedDatasChange.emit(this.selectedDatas)
    }
  }
  clearDatas(){
    this.clearCachedData()
    if(this.allowSelection){
      this.selectedDatas=[]
      this.selectedDatasChange.emit(this.selectedDatas)
    }
  
  }
  clearCachedData(){
    this.cachedData=[]
    this._query.pagination.skip=0
  }


  async loadNext(){
   await this.startLoading(async ()=>{
      if(this.cachedData.length<this.totalCount){
        this._query.pagination.skip+=this._query.pagination.take
        await this.dataSource.load()
      }
    },"loadNext")
  }


  private createDataSource() {
    return new DataSource<TOutputListDto, string>({
      load: async options => {
          const response = await lastValueFrom(this.getMethod(this.query));
          response.items.forEach(i => {
            if (
              (!this.allowSelection || this.selectedDatas.find(d => d.id == i.id) == undefined) &&
              !this.filteredKeys.includes(i.id)
            ) {
              this.cachedData.push(i);
            }
          });
          this.totalCount = response.totalCount;
          return { data: response.items, totalCount: response.totalCount };
      },
      key: ['id'],
      paginate: true,
    });
  }
}

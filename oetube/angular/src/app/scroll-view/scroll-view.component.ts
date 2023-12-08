import {
  Component,
  ContentChild,
  ContentChildren,

  EventEmitter,
  Input,
  OnInit,
  Output,
  AfterViewInit,
  QueryList,
  TemplateRef,
  AfterContentInit,
  ViewChildren,
  ViewChild,
  forwardRef,
  Directive,
  ChangeDetectorRef,
} from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { findIndex, lastValueFrom, Observable } from 'rxjs';
import { PaginationDto, QueryDto } from '@proxy/application/dtos';
import {
  DxDateRangeBoxComponent,
  DxScrollViewComponent,
  DxTextBoxComponent,
} from 'devextreme-angular';
import { Pagination } from '@proxy/domain/repositories/query-args';
import { ScrollEvent } from 'devextreme/ui/scroll_view';
import {
  DropDownSearchComponent,
  Filter,
  SortingMode,
} from './drop-down-search/drop-down-search.component';
import { DxiItemComponent } from 'devextreme-angular/ui/nested';
import { DragEndEvent } from 'devextreme/ui/draggable';
import { AddEvent, DragStartEvent } from 'devextreme/ui/sortable';
import { ItemClickEvent, ItemDeletedEvent } from 'devextreme/ui/list';
import type { EntityDto } from '@abp/ng.core';

@Directive({
  selector: '[appScrollViewContentTemplate]',
})
export class ScrollViewContentDirective implements Partial<DxiItemComponent> {
  private _appScrollViewContentTemplate: any[];
  @Input() set appScrollViewContentTemplate(value: any[]) {
    this._appScrollViewContentTemplate = value;
  }
  private _appScrollViewContentTemplateIcon: string;

  @Input() set appScrollViewContentTemplateIcon(value: string) {
    this._appScrollViewContentTemplateIcon = value;
  }
  get icon() {
    return this._appScrollViewContentTemplateIcon;
  }
  private _appScrollViewContentTemplateHint: string;
  @Input() set appScrollViewContentTemplateHint(value: string) {
    this._appScrollViewContentTemplateHint = value;
  }
  get hint() {
    return this._appScrollViewContentTemplateHint;
  }
  private _appScrollViewContentTemplateLayoutClassList: string;
  @Input() set appScrollViewContentTemplateLayoutClassList(value: string) {
    this._appScrollViewContentTemplateLayoutClassList = value;
  }
  get layoutClassList(): string {
    return this._appScrollViewContentTemplateLayoutClassList;
  }
  constructor(public contentTemplate: TemplateRef<Array<any>>) {}
}


@Component({
  selector: 'app-scroll-view-provider',
  template: '',
})
export abstract class ScrollViewProviderComponent<
  TOutputListDto extends EntityDto<string> = EntityDto<string>> implements AfterViewInit
{
 protected _options:ScrollViewOptions<TOutputListDto>={}
@Input() set options(v:ScrollViewOptions<TOutputListDto>){
  this._options=v
}
get options(){
  return this._options
}
abstract get scrollView():ScrollViewComponent<TOutputListDto>
setOptions(){

}
ngAfterViewInit(): void {
  this.setOptions()
  this.scrollView.options=this.options
  this.scrollView.changeDetector.detectChanges()
}

}


@Component({
  selector: 'app-scroll-view',
  templateUrl: './scroll-view.component.html',
  styleUrls: ['./scroll-view.component.scss'],
  providers: [
    { provide: ScrollViewProviderComponent, useExisting: forwardRef(() => ScrollViewComponent) },
  ],
})
export class ScrollViewComponent<
  TOutputListDto extends EntityDto<string> = EntityDto<string>
> extends ScrollViewProviderComponent<TOutputListDto> implements ScrollViewOptions, AfterViewInit{


  get scrollView(): ScrollViewComponent<TOutputListDto> {
      return this
  }
   _options:ScrollViewOptions<TOutputListDto>
  get options(){
    return this._options
  }
  @Output() optionsChange=new EventEmitter<ScrollViewOptions<TOutputListDto>>()
  @Input() set options(v:ScrollViewOptions<TOutputListDto>){
    Object.keys(v).forEach(k=>{
      if(k in ScrollViewOptionKeys){
        this[k]=v[k]
        this.options[k]=v[k]
        }
      })
      this.optionsChange.emit(this._options)
  }

  
  private _filteredKeys : string[]=[]
  public get filteredKeys() : string[] {
    return this._filteredKeys;
  }
  public set filteredKeys(v : string[]) {
    this._filteredKeys = v;
    this.filteredKeysChange.emit(v)
    this.refresh()
  }
  
  @Output() filteredKeysChange=new EventEmitter<string[]>();

  private _allowSelection=false
  @Output() allowSelectionChange=new EventEmitter<boolean>()
  @Input() set allowSelection(v:boolean){
    if(this._allowSelection!=v){
      this._allowSelection=v
      this.allowSelectionChange.emit(v)
    }
  }
  get allowSelection(){
    return this._allowSelection
  }
  private _selectedDatas : TOutputListDto[]=[]
  public get selectedDatas() :  TOutputListDto[] {
    return this._selectedDatas;
  }
  @Input() public set selectedDatas(v :  TOutputListDto[]) {
    this._selectedDatas = v;
    this.selectedDatasChange.emit(v)
    this.refresh()
  }
  @Output() selectedDatasChange=new EventEmitter<TOutputListDto[]>()  
  
  private _selectedDatasDisplayExpr : string="name"
  public get selectedDatasDisplayExpr() : string {
    return this._selectedDatasDisplayExpr;
  }
  public set selectedDatasDisplayExpr(v : string) {
    if(this._selectedDatasDisplayExpr!=v){
      this._selectedDatasDisplayExpr = v;
      this.selectedDatasDisplayExprChange.emit(v)
    }
  }
  @Output() selectedDatasDisplayExprChange=new EventEmitter<string>()

  
  private _getList? :  (args: QueryDto) => Observable<PaginationDto<TOutputListDto>>
  public get getList() :  (args: QueryDto) => Observable<PaginationDto<TOutputListDto>> {
    return this._getList;
  }
  public set getList(v :  (args: QueryDto) => Observable<PaginationDto<TOutputListDto>>) {
    this._getList = v;
    this.getListChange.emit(v)
    this.refresh().finally()
  }
  @Output() getListChange=new EventEmitter< (args: QueryDto) => Observable<PaginationDto<TOutputListDto>>>

  
  private _actionButtons : ActionButton[]=[];
  public get actionButtons() : ActionButton[]{
    return this._actionButtons;
  }
  public set actionButtons(v : ActionButton[]) {
    this._actionButtons = v;
    this.actionButtonsChange.emit(v)
  }
  @Output() actionButtonsChange=new EventEmitter<ActionButton[]>()

  
  private _height =450
  public get height() : number {
    return this._height;
  }
  public set height(v : number) {
    if(v!=this._height){
      this._height = v;
      this.heightChange.emit(v)
    }
  }
  @Output() heightChange=new EventEmitter<number>()

  
  private _creatorId? : string;
  public get creatorId() : string {
    return this._creatorId;
  }
  public set creatorId(v : string) {
    if(this._creatorId!=v){
      this._creatorId = v;
      this.query["creatorId"]=v
      this.creatorIdChange.emit(v)
      this.refresh().finally()
    }
  }
  @Output() creatorIdChange=new EventEmitter<string>()
  
  private _takePerLoad : number=20
  public get takePerLoad() : number {
    return this._takePerLoad;
  }
  public set takePerLoad(v : number) {
    if(this._takePerLoad!=v){
      this._takePerLoad = v;
      this.query.pagination.take=v
      this.takePerLoadChange.emit(v)
      this.refresh().finally()
    }
  }
  @Output() takePerLoadChange=new EventEmitter<number>()
  query: QueryDto={
    pagination:{
      skip:0,
      take:this._takePerLoad
    },sorting:""
  }
  @ViewChild(DxScrollViewComponent) instance: DxScrollViewComponent;
  @ContentChild(DropDownSearchComponent, { static: true }) dropDownSearch: DropDownSearchComponent;
  @ContentChildren(ScrollViewContentDirective) contentsQuery: QueryList<ScrollViewContentDirective>;
  dataSource: DataSource<TOutputListDto, string>;
  totalCount: number;
  cachedData: TOutputListDto[] = [];
  activeFilters: Filter[] = [];
  contents: ScrollViewContentDirective[];
  selectedContents: ScrollViewContentDirective[];
  constructor(public changeDetector:ChangeDetectorRef){
    super()
  }
  ngAfterViewInit(): void {
    this.refresh()
    this.changeDetector.detectChanges()
  }
  async refresh(){
    if(!this.getList){
      console.log("getList undefined")
      return
    }
    if(!this.dataSource){
      this.dataSource=this.createDataSource()
      this.dropDownSearch.filtersChange.subscribe(f => this.onFiltersChange(f));
      this.dropDownSearch.sortingModeChange.subscribe(s => this.onSortingChange(s));
      this.selectedContents = [this.contentsQuery.first];
      this.contents = this.contentsQuery.map(c => c);
    }
    this.instance.lockWidgetUpdate();
    this.query.pagination.skip = 0;
    this.cachedData = [];
    await this.dataSource.load();
    this.instance.unlockWidgetUpdate();

  }

  private createDataSource() {
    return new DataSource<TOutputListDto, string>({
      load: async options => {
        try {
          const response = await lastValueFrom(this.getList(this.query));
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
        } catch (error) {
          console.log(error);
        }
      },
      key: ['id'],
      paginate: true,
    });
  }

  removeFilter(key: string) {
    this.dropDownSearch.removeFilter(key);
  }
  async onFiltersChange(filters: Filter[]) {
    this.activeFilters = filters.filter(f => !f.isEmpty);
    filters.forEach(f => {
      Object.keys(f.value).forEach(k => {
        this.query[k] = f.value[k];
      });
    });
    await this.refresh();
  }

  async onSortingChange(sorting: SortingMode) {
    if (sorting.searchItemKey) {
      const value = sorting.value ? 'asc' : 'desc';
      this.query.sorting = sorting.searchItemKey + ' ' + value;
    } else {
      this.query.sorting = undefined;
    }
    await this.refresh();
  }

  scrollTop() {
    this.instance.instance.scrollTo(0);
  }

 
  reachedTop: boolean = true;
  reachedBottom: boolean;

  async onScroll(e: ScrollEvent) {
    this.reachedBottom = e.reachedBottom;
    this.reachedTop = e.reachedTop;
    if (this.reachedBottom && this.cachedData.length < this.totalCount) {
      const pagination: Pagination = {
        skip: this.query.pagination.skip + this.query.pagination.take,
        take: this.query.pagination.take,
      };
      this.query.pagination = pagination;
      await this.dataSource.load();
    }
  }

  onDragStart(e: DragStartEvent, sourceArray?: any[]) {
    if (!this.allowSelection) {
      e.cancel = true;
    }
    e.itemData = sourceArray;
  }
  onDragEnd(e: DragEndEvent) {}
  onSelectedDataDelete(e: ItemDeletedEvent) {
    if (this.selectedDataDisplay == e.itemData) {
      this.selectedDataDisplay = undefined;
    }
    this.cachedData.unshift(e.itemData);
  }
  selectedDatasClear() {
    this.cachedData.unshift(...this.selectedDatas);
    this.selectedDatas = [];
  }
  selectedDataDisplayVisible: boolean = false;
  selectedDataDisplay?: any;
  onSelectedDataClick(e: ItemClickEvent) {
    this.selectedDataDisplay = e.itemData;
    this.selectedDataDisplayVisible = true;
  }
  onDataAdd(e: AddEvent) {
    const source: any[] = e.itemData;
    source.splice(source.indexOf(e.fromData), 1);
    e.toData.push(e.fromData);
  }
}


export const ScrollViewOptionKeys:Readonly<Record<(keyof ScrollViewOptions),any>>={
actionButtons:undefined,
allowSelection:undefined,
creatorId:undefined,
filteredKeys:undefined,
getList:undefined,
height:undefined,
selectedDatas:undefined,
selectedDatasDisplayExpr:undefined,
takePerLoad:undefined
}

export interface ScrollViewOptions<TOutputListDto extends EntityDto<string> = EntityDto<string>>{
  filteredKeys?: string[];
  allowSelection?: boolean;
  selectedDatas?: TOutputListDto[];
  selectedDatasDisplayExpr?: string;
  getList?: (args: QueryDto) => Observable<PaginationDto<TOutputListDto>>;
  actionButtons?: ActionButton[];
  height?: number;
  creatorId?: string;
  takePerLoad?: number;
};

export type ActionButton = {
  icon: string;
  name: string;
  action: () => void;
  disabled?: () => boolean;
};
export class QueryTakeMax implements QueryDto {
  pagination: Pagination = {
    skip: 0,
    take: 2 << 32,
  };
}

import {
  Component,
  ContentChild,
  ContentChildren,
  EventEmitter,
  Input,
  OnInit,
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
  TOutputListDto extends EntityDto<string> = EntityDto<string>> 
  implements ScrollViewOptions<TOutputListDto
>, AfterViewInit
{
 @Input() isChild:boolean=false
 @Input() filteredKeys?: string[]
 @Input() allowSelection?:boolean
 @Input() selectedDatas?: TOutputListDto[]
 @Input() selectedDatasDisplayExpr?: string
 @Input() getList?: (args: QueryDto) => Observable<PaginationDto<TOutputListDto>>;
 @Input() actionButtons?:ActionButton[]
 @Input() height?:number
 @Input() creatorId?: string;
 @Input() takePerLoad?: number;
abstract get provider():ScrollViewProviderComponent<TOutputListDto>
abstract get scrollView():ScrollViewComponent<TOutputListDto>


protected loadOptions(options:ScrollViewOptions){
  Object.keys(options).forEach(k=>{
    this[k]=options[k]
  })
}
protected mergeToOptions(options?:ScrollViewOptions){
  Object.keys(this).forEach(k=>{
    if(k in ScrollViewOptionKeys && this[k]!=undefined && options[k]==undefined){
      options[k]=this[k]
    }
  })
}
initThis(){
}
initScrollView(options?:ScrollViewOptions){
  options=options??{}
  this.initThis()
  this.mergeToOptions(options)
  this.provider.initScrollView(options)
}
ngAfterViewInit(): void {
  if(!this.isChild){
    this.initScrollView()
  }
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
  options: ScrollViewOptions<TOutputListDto> = {
    filteredKeys: [],
    allowSelection: true,
    selectedDatasDisplayExpr: 'name',
    actionButtons: [],
    selectedDatas:[],
    height: 450,
    takePerLoad: 20,
  };
  get provider(): ScrollViewProviderComponent<TOutputListDto> {
    return this    
  }
  get scrollView(): ScrollViewComponent<TOutputListDto> {
    return this    
  }
  @ViewChild(DxScrollViewComponent) instance: DxScrollViewComponent;
  @ContentChild(DropDownSearchComponent, { static: true }) dropDownSearch: DropDownSearchComponent;
  @ContentChildren(ScrollViewContentDirective) contentsQuery: QueryList<ScrollViewContentDirective>;

  @Input() filteredKeys: string[]=[];
  @Input() allowSelection: boolean=false;
  @Input() selectedDatas: TOutputListDto[]=[];
  @Input() selectedDatasDisplayExpr: string="name"
  @Input() getList?: (args: QueryDto) => Observable<PaginationDto<TOutputListDto>>;
  @Input() actionButtons: ActionButton[]=[];
  @Input() height: number=450;
  @Input() creatorId?: string;
  @Input() takePerLoad:number=20

  query: QueryDto;

  dataSource: DataSource<TOutputListDto, string>;
  totalCount: number;
  cachedData: TOutputListDto[] = [];
  activeFilters: Filter[] = [];
  contents: ScrollViewContentDirective[];
  selectedContents: ScrollViewContentDirective[];
  initialized=false
  constructor(private cdr:ChangeDetectorRef){
    super()
  }
  initScrollView(options?:ScrollViewOptions){
    if(options){
      this.loadOptions(options)
    }

    if(!this.initialized &&this.getList){
      console.log("init")
      this.query = { pagination: { skip: 0, take: this.takePerLoad }, sorting: '' };
      if (this.creatorId) {
        this.query['creatorId'] = this.creatorId;
      }
      this.dataSource = this.createDataSource();
      this.dataSource.load().then();
      this.dropDownSearch.filtersChange.subscribe(f => this.onFiltersChange(f));
      this.dropDownSearch.sortingModeChange.subscribe(s => this.onSortingChange(s));
      this.selectedContents = [this.contentsQuery.first];
      this.contents = this.contentsQuery.map(c => c);
      this.initialized=true
    }
    this.cdr.detectChanges()
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

  async refresh() {
    this.instance.lockWidgetUpdate();
    this.query.pagination.skip = 0;
    this.cachedData = [];
    await this.dataSource.load();
    this.instance.unlockWidgetUpdate();
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

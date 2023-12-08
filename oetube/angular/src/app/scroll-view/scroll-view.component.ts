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
  ElementRef,
  ViewContainerRef,
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
import {
  AppTemplate,
} from '../template-ref-collection/template-ref-collection.component';
import {
  DataSourceProviderDirective,
  ScrollViewDataSourceComponent,
} from './scroll-view-data-source/scroll-view-data-source.component';
import {
  ScrollViewContentsComponent,
} from './scroll-view-contents/scroll-view-contents.component';

@Component({
  selector: 'app-scroll-view',
  templateUrl: './scroll-view.component.html',
  styleUrls: ['./scroll-view.component.scss'],
})
export class ScrollViewComponent<TOutputListDto extends EntityDto<string> = EntityDto<string>>
  implements AfterViewInit
{
  @Input() initialLoad = true;
  @Input() selectedDatasDisplayExpr: string = 'name';

  @Input() actionButtons: ActionButton[] = [];
  @Input() height = 400;

  @ViewChild(DxScrollViewComponent) instance: DxScrollViewComponent;

  @ContentChild(DropDownSearchComponent,{descendants:true}) dropDownSearch: DropDownSearchComponent;
  @ViewChild("search") searchContainer: ElementRef<any>


  @ContentChild(DataSourceProviderDirective)
  public dataSourceProvider: DataSourceProviderDirective<TOutputListDto>;

  @ContentChild(DropDownSearchComponent) dataSource: ScrollViewDataSourceComponent<TOutputListDto>;
  @ContentChild(ScrollViewContentsComponent) contents: ScrollViewContentsComponent;

  activeFilters: Filter[] = [];

  constructor(private changeDetector:ChangeDetectorRef){
  }

  async ngAfterViewInit() {
    debugger
    this.changeDetector.detach()
    if(this.dropDownSearch){
        this.dropDownSearch.filtersChange.subscribe(f => this.onFiltersChange(f));
        this.dropDownSearch.sortingModeChange.subscribe(s => this.onSortingChange(s));
    }

    if (this.dataSourceProvider) {
      this.dataSource = this.dataSourceProvider.scrollViewDataSourceComponent;
      if (this.initialLoad) {
        await this.dataSource.reload();
      }

      this.dataSource.beginLoad.subscribe(() => {
        this.instance.lockWidgetUpdate();
      });
      this.dataSource.endLoad.subscribe(() => {
        this.instance.unlockWidgetUpdate();
      });
    }
      this.changeDetector.reattach()
    this.changeDetector.detectChanges()
  }
  removeFilter(key: string) {
    this.dropDownSearch.removeFilter(key);
    this.dataSource.refresh();
  }
  async onFiltersChange(filters: Filter[]) {
    this.activeFilters = filters.filter(f => !f.isEmpty);
    filters.forEach(f => {
      Object.keys(f.value).forEach(k => {
        this.dataSource.query[k] = f.value[k];
      });
    });
    await this.dataSource.refresh();
  }

  async onSortingChange(sorting: SortingMode) {
    if (sorting.searchItemKey) {
      const value = sorting.value ? 'asc' : 'desc';
      this.dataSource.query.sorting = sorting.searchItemKey + ' ' + value;
    } else {
      this.dataSource.query.sorting = undefined;
    }
    await this.dataSource.refresh();
  }

  async refresh() {
    await this.dataSource.refresh();
  }
  scrollTop() {
    this.instance.instance.scrollTo(0);
    this.dataSource.refresh();
  }

  reachedTop: boolean = true;
  reachedBottom: boolean;

  async onScroll(e: ScrollEvent) {
    this.reachedBottom = e.reachedBottom;
    this.reachedTop = e.reachedTop;
    if (this.reachedBottom) {
      await this.dataSource.loadNext();
    }
  }

  onDragStart(e: DragStartEvent) {
    console.log(this)
    if (!this.dataSource.allowSelection) {
      e.cancel = true;
    }
    e.itemData=this
  }

  onSelectedDataDelete(e: ItemDeletedEvent) {
    debugger
    if (this.selectedDataDisplay == e.itemData) {
      this.selectedDataDisplay = undefined;
    }
    this.dataSource.deselectItem(e.itemData);
  }
  selectedDatasClear() {
    this.dataSource.clearSelection();
  }

  selectedDataDisplayVisible: boolean = false;
  selectedDataDisplay?: any;
  onSelectedDataClick(e: ItemClickEvent) {
    this.selectedDataDisplay = e.itemData;
    this.selectedDataDisplayVisible = true;
  }
  onDataAdd(e: AddEvent) {
    const _this:ScrollViewComponent=e.itemData


    _this.dataSource.selectItem(e.fromData);
  }
}

export type ActionButton = {
  icon: string;
  name: string;
  action: () => void;
  disabled?: () => boolean;
};

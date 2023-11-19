import { Component, Input, OnInit, ElementRef, ViewChild,Output, EventEmitter} from '@angular/core';
import { lastValueFrom, Observable } from 'rxjs';
import { PaginationDto, QueryDto } from '@proxy/application/dtos';
import DataSource from 'devextreme/data/data_source';
import { LoadOptions } from 'devextreme/data';
import { DxDataGridComponent } from 'devextreme-angular';
import { DxiButtonComponent, DxoEditingComponent, DxoPagerComponent, DxoPagingComponent, DxoSelectionComponent } from 'devextreme-angular/ui/nested';
import dxDataGrid from 'devextreme/ui/data_grid';
@Component({
  template: '',
})
export abstract class PaginationGridComponent<
  TQueryArgs extends QueryDto,
  TOutputDto,
  TOutputListDto,
  TUpdateDto
> implements OnInit
{
  

  defaultItemPerPage: number = 10;
  pageSizes: Array<number> = [this.defaultItemPerPage, 20, 50, 100];

  @ViewChild(DxDataGridComponent, {static:true}) dxDataGrid:DxDataGridComponent

  @Input() selectedKeys: Array<string>
  @Output() selectedKeysChange: EventEmitter<Array<string>> = new EventEmitter<Array<string>>()

  @Input() updateModel: TUpdateDto
  @Output() updateModelChanged: EventEmitter<TUpdateDto> = new EventEmitter<TUpdateDto>()

  @Input() height: number
  @Input() width: number
  @Input() queryArgs: TQueryArgs = { itemPerPage: this.defaultItemPerPage, page: 0 } as TQueryArgs
  @Input() selectionOptions: SelectionOptions = new SelectionOptions()
  @Input() pagerOptions: PagerOptions = new PagerOptions()
  @Input() pagingOptions: PagingOptions = new PagingOptions()
  @Input() dataGridOptions: DataGridOptions = new DataGridOptions()
  @Input() editorOptions: EditorOptions = new EditorOptions()
  @Input() filterOptions: FilterOptions=new FilterOptions()
  @Input() showId:boolean=true

  dataSource: DataSource<TOutputListDto, string>
  allowedPageSizes:Array<number>=[]

  ngOnInit(): void {
    this.initDataSource()
    this.initDataGrid()
  }

  initDataSource() {
    this.dataSource = new DataSource({
      load: async options => {
        if (this.queryArgs.itemPerPage == null) {
          this.queryArgs.itemPerPage = this.defaultItemPerPage
        }
        this.handlePagination(options);
        this.handleFilter(options);
        if (options.sort instanceof Array && options.sort.length > 0) {
          this.handleSorting(options.sort[0] as { selector: string; desc: boolean });
        }

        return lastValueFrom(this.getList())
          .then(response => {
            this.setAllowedPageSizes(response.totalCount);
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
      remove: async (key) => {
        await lastValueFrom(this.delete(key))
      },
      update: async (key, values) => {
        this.updateModel = this.mapOutputToUpdate(await lastValueFrom(this.update()))
      }
    });
  }
  mapObject(source:any,destination:any){
    Object.keys(source).forEach(k => {
      destination[k] = source[k]
    })
  }
  nameof = (name: Extract<keyof DxDataGridComponent, string>): string => name;
  mapOptions(source:any,dataGridpropertyName:string) {
    debugger;
    if(!this.dxDataGrid[dataGridpropertyName]){
      this.dxDataGrid[dataGridpropertyName]=new Object()
    }
    this.mapObject(source,this.dxDataGrid[dataGridpropertyName])
  }

  initDataGrid() {
    this.dxDataGrid.dataSource=this.dataSource
    this.dxDataGrid.width=this.width
    this.dxDataGrid.height=this.height
    this.mapObject(this.dataGridOptions,this.dxDataGrid)
    this.mapOptions(this.filterOptions,this.nameof('filterRow'))
    this.mapOptions(this.editorOptions,this.nameof('editing'))
    this.dxDataGrid.paging=this.pagingOptions
    this.dxDataGrid.pager=this.pagerOptions
    this.dxDataGrid.selection=this.selectionOptions
    if (this.selectedKeys) {
      this.dxDataGrid.selectedRowKeys = this.selectedKeys.map(i => { id: i })
    }
   this.dxDataGrid.selectedRowKeysChange.subscribe((value: { id: string }[]) => {
      this.selectedKeysChange.emit(value.map(item => item.id))
    })
  }
  abstract getList(): Observable<PaginationDto<TOutputListDto>>

  getDetails(key: string): Observable<TOutputDto> {
    return undefined
  }
  delete(key: string): Observable<any> {
    return undefined
  }

  mapOutputToUpdate(dto: TOutputDto): TUpdateDto {
    return undefined
  }


  update(): Observable<TOutputDto> {
    return undefined
  }
  setAllowedPageSizes(totalCount: number) {
    debugger;
    this.dxDataGrid.pager.allowedPageSizes = [this.pageSizes[0]];
    for (let index = 1; index < this.pageSizes.length; index++) {
      const element = this.pageSizes[index];
      if (element > totalCount) {
        break;
      }
      this.dxDataGrid.pager.allowedPageSizes.push(element);
    }
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
}

export class FilterOptions{
  visible:boolean=true

}
export class EditorOptions {
  allowUpdating: boolean = true
  allowDeleting: boolean = true
  mode: string = "row"
  useIcons: boolean = true

}
export class DataGridOptions {
  remoteOperations: boolean = true
  cacheEnabled: boolean = true
  showBorders: boolean = true
  allowColumnResizing: boolean = true
  columnAutoWidth: boolean = true
  columnResizingMode: string = "nextColumn"
  showColumnHeaders: boolean = true
}
export class SelectionOptions {
  allowSelectAll: boolean = true
  mode: string = "multiple"
  showCheckBoxesMode: string = "always"
  selectAllMode: string = "page"
}
export class PagingOptions{
  enabled:boolean=true
}
export class PagerOptions {
  showInfo: boolean = true
  showNavigationButtons: boolean = true
  visible: boolean = true
  showPageSizeSelector: boolean = true
}
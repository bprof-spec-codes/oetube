import { Rest } from '@abp/ng.core';
import { HttpParameterCodec } from '@angular/common/http';
import { Component,OnInit, ViewChild,ElementRef } from '@angular/core';
import { PaginationDto } from '@proxy/application/dtos';
import DevExpress from 'devextreme';
import { DxDataGridComponent } from 'devextreme-angular';
import { DxoFormSimpleItem } from 'devextreme-angular/ui/nested/base/form-simple-item';
import { LoadOptions, Store } from 'devextreme/data';
import DataSource  from 'devextreme/data/data_source';
import { ColumnHeaderFilterSearchConfig} from 'devextreme/ui/data_grid';
import {lastValueFrom, Observable } from 'rxjs';


@Component({
  selector: 'app-pagination-grid',
  templateUrl: './pagination-grid.component.html',
  styleUrls: ['./pagination-grid.component.scss']
})

export class PaginationGridComponent<TInput extends QueryArgs,TOutput> implements OnInit{


  @ViewChild("dataGrid") dataGrid:ElementRef<DxDataGridComponent>
  dataSource: DataSource
  totalCount:number
  columns:Array<DataGridColumn>
  listInput:TInput
  


   getList(input:TInput,config?:Partial<Partial<{
    apiName: string;
    skipHandleError: boolean;
    skipAddingHeader: boolean;
    observe: Rest.Observe;
    httpParamEncoder?: HttpParameterCodec;
    }>>):Observable<PaginationDto<TOutput>>{
      return undefined
    }

  beforeLoad(options:LoadOptions,input:TInput){

  }
  initInput():TInput{
    return {itemPerPage:10,page:0} as TInput
  }
  initColumns():Array<DataGridColumn>{
    return []
  }

  initDataSource(){
    this.dataSource=new DataSource(
      {
        load:async (options)=>{
          this.beforeLoad(options,this.listInput)
          return lastValueFrom(this.getList(this.listInput))
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
      })
  }

  ngOnInit(): void {
    this.listInput=this.initInput()
    this.columns=this.initColumns()
    this.initDataSource()
  }

  onPagingChanged(e:PagingChangedEventArgs){
    this.listInput.itemPerPage=e.pageSize
    this.listInput.page=e.pageIndex
  }
}

export interface PagingChangedEventArgs{
  enabled?: boolean;
  pageIndex?: number;
  pageSize?: number;
}
export interface QueryArgs{
  itemPerPage?: number;
  page?: number;
  sorting?:string
}

export interface DataGridColumn {
  alignment?: string | undefined
  allowEditing?: boolean;
  allowExporting?: boolean;
  allowFiltering?: boolean;
  allowFixing?: boolean;
  allowGrouping?: boolean;
  allowHeaderFiltering?: boolean;
  allowHiding?: boolean;
  allowReordering?: boolean;
  allowResizing?: boolean;
  allowSearch?: boolean;
  allowSorting?: boolean;
  autoExpandGroup?: boolean;
  buttons?: Array<string>;
  calculateCellValue?: Function;
  calculateDisplayValue?: Function | string;
  calculateFilterExpression?: Function;
  calculateGroupValue?: Function | string;
  calculateSortValue?: Function | string;
  caption?: string | undefined;
  cellTemplate?: any;
  columns?: Array<string>;
  cssClass?: string | undefined;
  customizeText?: Function;
  dataField?: string | undefined;
  dataType?: string | undefined;
  editCellTemplate?: any;
  editorOptions?: any;
  encodeHtml?: boolean;
  falseText?: string;
  filterOperations?: Array<string>;
  filterType?: string;
  filterValue?: any | undefined;
  filterValues?: Array<any>;
  fixed?: boolean;
  fixedPosition?: string | undefined;
  format?: string;
  formItem?: DxoFormSimpleItem;
  groupCellTemplate?: any;
  groupIndex?: number | undefined;
  headerCellTemplate?: any;
  headerFilter?: {
      allowSearch?: boolean;
      allowSelectAll?: boolean;
      dataSource?:  Store | Function | null | undefined | Array<any>;
      groupInterval?: number | string | undefined;
      height?: number | undefined;
      search?: ColumnHeaderFilterSearchConfig;
      searchMode?: string;
      width?: number | undefined;
  };
  hidingPriority?: number | undefined;
  isBand?: boolean | undefined;
  lookup?: {
      allowClearing?: boolean;
      calculateCellValue?: Function;
      dataSource?: Store | Function | null | undefined | Array<any>;
      displayExpr?: Function | string | undefined;
      valueExpr?: string | undefined;
  };
  minWidth?: number | undefined;
  name?: string | undefined;
  ownerBand?: number | undefined;
  renderAsync?: boolean;
  selectedFilterOperation?: string | undefined;
  setCellValue?: Function;
  showEditorAlways?: boolean;
  showInColumnChooser?: boolean;
  showWhenGrouped?: boolean;
  sortIndex?: number | undefined;
  sortingMethod?: Function | undefined;
  sortOrder?: string | undefined;
  trueText?: string;
  type?: string;
  validationRules?: Array<DevExpress.common.RequiredRule | DevExpress.common.NumericRule | DevExpress.common.RangeRule | DevExpress.common.StringLengthRule | DevExpress.common.CustomRule | DevExpress.common.CompareRule | DevExpress.common.PatternRule | DevExpress.common.EmailRule | DevExpress.common.AsyncRule>;
  visible?: boolean;
  visibleIndex?: number | undefined;
  width?: number | string | undefined;
}

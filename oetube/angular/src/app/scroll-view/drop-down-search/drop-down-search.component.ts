
import { Type, TypeofExpr } from '@angular/compiler';
import {
  Component,
  ViewChildren,
  ViewChild,
  QueryList,
  EventEmitter,
  Input,
  Output,
  HostListener,
  OnInit,
  AfterContentInit,
  ContentChildren,
  forwardRef,
  HostBinding,
  Directive,
  Inject,
  ContentChild,
  Injector,
  TemplateRef,
  ChangeDetectorRef,
  ElementRef,
  Host,
  ViewContainerRef,
  AfterViewInit
} from '@angular/core';
import{ControlValueAccessor,NG_VALUE_ACCESSOR} from  "@angular/forms"
import { QueryDto } from '@proxy/application/dtos';
import { debug } from 'console';
import DevExpress from 'devextreme';
import { DxButtonComponent, DxButtonGroupComponent, DxDropDownButtonComponent, DxRangeSelectorComponent, DxRangeSliderComponent, DxTextBoxComponent } from 'devextreme-angular';
import { DxiItemComponent } from 'devextreme-angular/ui/nested';
import { DxiButtonGroupItem } from 'devextreme-angular/ui/nested/base/button-group-item-dxi';
import dxButton, { ClickEvent } from 'devextreme/ui/button';
import { SelectionChangedEvent } from 'devextreme/ui/drop_down_button';
import { isEmpty } from 'rxjs';
import { DeepCloner } from 'src/app/base-types/builder';
import { Converter } from 'src/app/base-types/converter';
import { Time, TimeInitType } from 'src/app/base-types/time';
import { AppTemplate, TemplateRefCollectionComponent } from 'src/app/template-ref-collection/template-ref-collection.component';
import { SearchItem } from './search-item';



@Component({
  selector: 'app-drop-down-search',
  templateUrl: './drop-down-search.component.html',
  styleUrls: ['./drop-down-search.component.scss'],
})
export class DropDownSearchComponent implements AfterViewInit{

  @ViewChild(DxDropDownButtonComponent) private dropDownButton:DxDropDownButtonComponent
  @Input() iconSortingAsc:string='sortuptext'
  @Input() iconSortingDesc:string='sortdowntext'
  @Input() iconSortingClear:string='isblank'
  @Input() iconSearch:string='search'
  @Output() filtersChange:EventEmitter<Filter[]>=new EventEmitter<Filter[]>()
  @Output() sortingModeChange:EventEmitter<SortingMode>=new EventEmitter<SortingMode>()
  @ContentChild(TemplateRefCollectionComponent<SearchItem>) templateCollection:TemplateRefCollectionComponent<SearchItem>

  constructor(private changeDetector:ChangeDetectorRef){

  }
  get items(){
    return this.templateCollection.items
  }
  get selectedItem():AppTemplate<SearchItem>{
    return this.templateCollection.selectedItem
  }
  set selectedItem(v:AppTemplate<SearchItem>){
    this.templateCollection.selectedItem=v
  }

  sortBtns:SortButton[]=[
    new SortButton(this.iconSortingAsc,true,"Sort Ascending"),
    new SortButton(this.iconSortingDesc,false,"Sort Descending"),
    new SortButton(this.iconSortingClear,undefined,"Clear Sorting")
  ]
  selectedSortBtn:SortButton[]=[]
  ngAfterViewInit(): void {
    if(this.items.length>0){
      this.changeDetector.detach()
      this.refreshSortBtns()
      this.items.forEach(i=>{
        i.enterKey.subscribe(()=>{
          this.raiseFilterChange()
        })
      })
      this.changeDetector.reattach()
      this.changeDetector.detectChanges()
    }
  }
  
  onSelectionChanged(e:SelectionChangedEvent){
    this.refreshSortBtns()
  }

  onItemClick(e){
    const itemData=e.itemData
    this.dropDownButton.text=itemData.display
  }

  private raiseFilterChange(){
    this.filtersChange.emit(this.items.map(i=>{
      const filter:Filter={
        display:i.display,
        key:i.key,
        value:i.value,
        isEmpty:i.isEmpty()
      }
      return filter
    }))
  }
  removeFilter(key:string){
    this.items.find(f=>f.key==key)?.clear()
    this.raiseFilterChange()
    }

  onSearchClick(){
    this.raiseFilterChange()
  }

  isSortBtnDisabled(value:boolean|undefined){
    return value!=undefined&&!this.selectedItem.allowSorting
  }

  isSortBtnSelected(value:boolean|undefined){
    return (this.sortingMode.searchItemKey==this.selectedItem.key||value==undefined)&&this.sortingMode.value==value
  }
  onSortBtnClick(e){
    const btn:SortButton=e.itemData
    if(this.isSortBtnSelected(btn.sortValue)||this.isSortBtnDisabled(btn.sortValue)){
      return
    }
    this.sortingMode.searchItemKey=btn.sortValue==undefined?undefined:this.selectedItem.key
    this.sortingMode.value=btn.sortValue
    this.refreshSortBtns()
    this.sortingModeChange.emit(this.sortingMode)
  }
  refreshSortBtns(){
    this.sortBtns.forEach(b=>{
      b.disabled=this.isSortBtnDisabled(b.sortValue)
    })
    const selectedBtn=this.sortBtns.find(b=>this.isSortBtnSelected(b.sortValue))
    if(selectedBtn)
    {
      this.selectedSortBtn=[selectedBtn]
    }
    else{
      this.selectedSortBtn=[]
    }
    this.sortBtns=[...this.sortBtns]
  }
  sortingMode:SortingMode={}

 
}
export type SortingMode={
  searchItemKey?:string
  value?:boolean
}
class SortButton implements Partial<DxiItemComponent>{
  disabled?:boolean
  constructor(public icon:string,public sortValue:boolean,public hint:string){
  }
}
export type Filter={
  key:string
  display:string
  value:Readonly<object>
  isEmpty:boolean
}


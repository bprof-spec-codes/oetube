
import { Type } from '@angular/compiler';
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
  Injector,
  ChangeDetectorRef
} from '@angular/core';
import{ControlValueAccessor,NG_VALUE_ACCESSOR} from  "@angular/forms"
import { QueryDto } from '@proxy/application/dtos';
import { debug } from 'console';
import DevExpress from 'devextreme';
import { DxButtonComponent, DxButtonGroupComponent, DxDropDownButtonComponent, DxTextBoxComponent } from 'devextreme-angular';
import { DxiItemComponent } from 'devextreme-angular/ui/nested';
import { DxiButtonGroupItem } from 'devextreme-angular/ui/nested/base/button-group-item-dxi';
import dxButton, { ClickEvent } from 'devextreme/ui/button';
import { SelectionChangedEvent } from 'devextreme/ui/drop_down_button';
import { isEmpty } from 'rxjs';
import { DeepCloner } from 'src/app/base-types/builder';
import { Converter } from 'src/app/base-types/converter';




@Directive({
  selector: '[appSearchItem]',
})
export class SearchItemDirective implements OnInit{
  protected _appSearchItem:any[]
  @Input() set appSearchItem(value:any[]){
    this._appSearchItem=value
  }

  protected _appSearchItemDisplay:string=""
  @Input() set appSearchItemDisplay(value:string){
    this._appSearchItemDisplay=value
  }
  get display(){
    return this._appSearchItemDisplay
  }
  protected _appSearchItemKey:string=""
  @Input() set appSearchItemKey(value:string){
    this._appSearchItemKey=value
  }
  get key(){
    return this._appSearchItemKey
  }
  protected _appSearchItemValueKeys?:string[]
  @Input() set appSearchItemValueKeys(value:string[]){
    this._appSearchItemValueKeys=value
  }
  get valueKeys():string[]|undefined{
    return this._appSearchItemValueKeys
  }

  protected _appSearchItemAllowSorting:boolean=true
  @Input() set appSearchItemAllowSorting(value:boolean){
    this._appSearchItemAllowSorting=value
  }
  get allowSorting(){
    return this._appSearchItemAllowSorting
  }
  protected _appSearchItemSourceName:string="value"
  @Input() set appSearchItemSourceName(value:string){
    this._appSearchItemSourceName=value
  }
  get sourceName(){
    return this._appSearchItemSourceName
  }
  protected _appSearchItemSourceChangeName:string="valueChange"
  @Input() set appSearchItemSourceChangeName(value:string){
    this._appSearchItemSourceChangeName=value
  }
  get sourceChangeName(){
    return this._appSearchItemSourceChangeName
  }
  protected _appSearchItemConverter?:Converter
  @Input() set appSearchItemConverter(value:Converter){
    this._appSearchItemConverter=value
  }
  get converter(){
    return this._appSearchItemConverter
  }

  protected keys:string[]
  protected element:ControlValueAccessor
  protected defaultValue:any
  constructor(@Inject(Injector) injector:Injector){
    this.element=injector.get(NG_VALUE_ACCESSOR)[0] as ControlValueAccessor
  }



  @HostBinding('value') value: any = new Object();

  @HostBinding('hidden') hidden:boolean=true
  @HostBinding('style.minHeight.px') protected minHeight = 10;
  @HostBinding('style.height.px') protected height = 40;
  @HostBinding('class.my-auto') protected myAuto = true;
  @HostBinding('class.mx-1') protected mx1 = true;
  @HostBinding('class.border') protected border = true;
  @HostBinding('class.rounded') protected rounded = true;
  @HostBinding('class.w-100') protected w100 = true;
  @HostListener('onEnterKey') protected enterKeyEvent(){
    this.enterKeyPressed.emit(this)
  }
  @Output() enterKeyPressed:EventEmitter<SearchItemDirective>=new EventEmitter<SearchItemDirective>()
  protected cloner:DeepCloner=new DeepCloner()


  ngOnInit(): void {
    this.initKeys()
    this.resetValue()
    this.defaultValue=this.cloner.Clone(this.element[this.sourceName])
    this.element[this.sourceChangeName].subscribe(v => {
      this.setValue(v);
    });
  }
  getConvertedValue(){
    if(this.converter){
      return this.converter.convert(this.value)
    }
    else{
      return this.value
    }
  }
  initKeys(){
    this.keys=[]
    if(this.valueKeys==undefined){
      this.keys.push(this.key)
    }
    else{
      this.valueKeys.forEach(k=>{
        this.keys.push(this.key+k)
      })
    }
  }
  private resetValue(){
    this.keys.forEach(k=>{
      this.value[k]=undefined
    })
  }

  isEmpty(){
    const keys=Object.keys(this.value)
    if(keys.length==0) return true
    for(const k in keys){
      const keyValue=this.value[keys[k]]
      if(keyValue!=undefined&&keyValue!=""){
        return false
      }
    }
   return true
  }
  public clear(){
    this.resetValue()
    this.element.writeValue(this.cloner.Clone(this.defaultValue))
  }
  private setValueMode: (v: any) => void;
  private setValue(v: any) {
    if (!this.setValueMode) {
    if(this.keys.length>0){
      if(v instanceof Array){
        this.setValueMode = (val: any) => {
          let i=0
          this.keys.forEach(k=> this.value[k]=val[i++])
          }
        }
        else{
          this.setValueMode=(val:any)=> this.value[this.keys[0]]=val
        }
    }
    else{
      this.setValueMode=(val:any)=>undefined
    }
  }
    this.setValueMode(v)
  }
}
@Directive({
  selector:"[appNameSearchItem]",
  providers:[{provide:SearchItemDirective,useExisting:forwardRef(()=>NameSearchItemDirective)}]
})
export class NameSearchItemDirective extends SearchItemDirective{
  protected _appSearchItemDisplay: string="Name"
  protected _appSearchItemKey: string="name"
}
@Directive({
  selector:"[appCreationTimeSearchItem]",
  providers:[{provide:SearchItemDirective,useExisting:forwardRef(()=>CreationTimeSearchItemDirective)}]
})
export class CreationTimeSearchItemDirective extends SearchItemDirective{
  protected _appSearchItemDisplay: string="Creation Time"
  protected _appSearchItemKey: string="creationTime"
  protected _appSearchItemValueKeys?: string[]=['Min','Max']
  @HostBinding("displayFormat") displayFormat="yyyy.MM.dd"
  protected _appSearchItemConverter?: Converter<any, any>={
    convert(v){
      let convertedValue={}
      Object.keys(v).forEach((k)=>{
        const keyValue:Date=v[k]
      if(keyValue!=undefined){
          convertedValue[k]=keyValue.toISOString()
        }
        else{
          convertedValue[k]=undefined
        }
      })
      return convertedValue
    }
  }
}
@Component({
  selector: 'app-drop-down-search',
  templateUrl: './drop-down-search.component.html',
  styleUrls: ['./drop-down-search.component.scss'],
})
export class DropDownSearchComponent implements AfterContentInit{
  @ViewChild(DxDropDownButtonComponent) private dropDownButton:DxDropDownButtonComponent
  @ContentChildren(SearchItemDirective) private contents: QueryList<SearchItemDirective>;
  @Input() iconSortingAsc:string='sortuptext'
  @Input() iconSortingDesc:string='sortdowntext'
  @Input() iconSortingClear:string='isblank'
  @Input() iconSearch:string='search'
  @Output() filtersChange:EventEmitter<Filter[]>=new EventEmitter<Filter[]>()
  @Output() sortingModeChange:EventEmitter<SortingMode>=new EventEmitter<SortingMode>()
  public hasItem:boolean=false
  public selectedItem:SearchItemDirective
  public selectedItemChange:EventEmitter<SearchItemDirective>=new EventEmitter<SearchItemDirective>()
  private searchItems:SearchItemDirective[]

  sortBtns:SortButton[]=[
    new SortButton(this.iconSortingAsc,true,"Sort Ascending"),
    new SortButton(this.iconSortingDesc,false,"Sort Descending"),
    new SortButton(this.iconSortingClear,undefined,"Clear Sorting")
  ]
  selectedSortBtn:SortButton[]=[]

  ngAfterContentInit(): void {
    this.hasItem=this.contents && this.contents.length>0
    if(this.hasItem){
      this.selectedItem=this.contents.first
      this.selectedItem.hidden=false
      this.searchItems=this.contents.map(i=>i)
      this.searchItems.forEach(i=>{
        i.enterKeyPressed.subscribe(()=>this.raiseFilterChange())
      })
      this.refreshSortBtns() 
  }
  }


  onSelectionChanged(e:SelectionChangedEvent){
    const prev:SearchItemDirective=e.previousItem
    const current:SearchItemDirective=e.item
    prev.hidden=true
    current.hidden=false
    this.refreshSortBtns()
    this.selectedItemChange.emit(this.selectedItem)
  }

  onItemClick(e){
    const itemData:SearchItemDirective=e.itemData
    this.dropDownButton.text=itemData.display
  }

  private raiseFilterChange(){
    this.filtersChange.emit(this.searchItems.map(i=>{
      const filter:Filter={
        display:i.display,
        key:i.key,
        value:i.getConvertedValue(),
        isEmpty:i.isEmpty()
      }
      return filter
    }))
  }
  removeFilter(key:string){
    this.searchItems.find(f=>f.key==key)?.clear()
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
  constructor(private changeDetector:ChangeDetectorRef){
  }

 
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

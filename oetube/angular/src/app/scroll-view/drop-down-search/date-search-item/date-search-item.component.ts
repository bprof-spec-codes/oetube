import { Component,Input,OnInit } from '@angular/core';
import { debug } from 'console';
import { Converter } from 'src/app/base-types/converter';
import { SearchItem } from '../search-item';

@Component({
  selector: 'app-date-search-item',
  templateUrl: './date-search-item.component.html',
  styleUrls: ['./date-search-item.component.scss']
})
export class DateSearchItemComponent implements OnInit  {
  @Input() searchItem:SearchItem
  @Input() displayFormat="yyyy.MM.dd"
  protected defaultConverter?: Converter<any, any>={
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
  
  protected defaultValueKeys?: string[]=['Min','Max']
  ngOnInit(): void {
  this.searchItem.converter??=this.defaultConverter
  this.searchItem.valueKeys??=this.defaultValueKeys
  this.searchItem.init(this.searchItem)
  }


 
}

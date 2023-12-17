
import { DeepCloner } from 'src/app/base-types/builder';
import { Converter } from 'src/app/base-types/converter';
import {EventEmitter} from '@angular/core';

type SearchItemOptions=Partial<Pick<SearchItem,'key'|'display'|'valueKeys'|'allowSorting'|'converter'|'height'|'class'>>

export class SearchItem {
  key: string;
  display: string;
  valueKeys: string[];
  allowSorting: boolean=true
  converter: Converter;
  height:number=40
  private minHeight:number=10
  class:string="my-auto rounded border"
  style:string
  
  enterKey:EventEmitter<any>=new EventEmitter


  protected cloner: DeepCloner = new DeepCloner();
  protected keys: string[];

  protected _value: Object={}
  get value(): Object {
    if (this.converter) {
      return this.converter.convert(this._value);
    } else {
      return this._value;
    }
  }

  protected setValueMode: (v: any) => void;
  protected setValue(v: any) {
    if (!this.setValueMode) {
      if (this.keys.length > 0) {
        if (v instanceof Array) {
          this.setValueMode = (val: any) => {
            let i = 0;
            this.keys.forEach(k => (this._value[k] = val[i++]));
          };
        } else {
          this.setValueMode = (val: any) => (this._value[this.keys[0]] = val);
        }
      } else {
        this.setValueMode = (val: any) => undefined;
      }
    }
    this.setValueMode(v);
  }

  protected _bindingValue=[]
  set bindingValue(v: any) {
    this._bindingValue = v;
    this.setValue(v);
  }
  get bindingValue() {
    return this._bindingValue;
  }


public init(options:SearchItemOptions){
    this._value={}
    this.setValueMode=undefined
    Object.keys(options).forEach(k=>{
        this[k]=options[k]
    })
    this.style=`height: ${this.height}px; min-height: ${this.minHeight}px`
    this.initKeys();
    this.resetValue();
    return this
}

 private initKeys() {
    this.keys = [];
    if (this.valueKeys == undefined) {
      this.keys.push(this.key);
    } else {
      this.valueKeys.forEach(k => {
        this.keys.push(this.key + k);
      });
    }
  }
  private resetValue() {
    this.keys.forEach(k => {
      this._value[k] = undefined;
    });
  }

  public isEmpty() {
    const keys = Object.keys(this.value);
    if (keys.length == 0) return true;
    for (const k in keys) {
      const keyValue = this.value[keys[k]];
      if (keyValue != undefined && keyValue != '') {
        return false;
      }
    }
    return true;
  }
   public clear() {
    this.resetValue();
    this._bindingValue =[]
  }
  onEnterKey(){
    console.log(this.value)
    this.enterKey.emit()
  }


}

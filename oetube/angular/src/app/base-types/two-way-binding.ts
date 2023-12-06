import { EventEmitter } from '@angular/core';
import { PaginationDto } from '@proxy/application/dtos';
import { GroupListItemDto } from '@proxy/application/dtos/groups';
import { UserListItemDto } from '@proxy/application/dtos/oe-tube-users';
import { Column } from 'devextreme/ui/data_grid';
import { Converter } from './converter';
export interface NotifyInit {
  initialized?: EventEmitter<any>;
}


export class PropertySelector<T>{
  variable:T
  path:string[]
  parent:any

  get key():string|undefined{
      if(this.path.length==0){
          return undefined
      }
      return this.path[this.path.length-1]
  }
  constructor(variable:T,path?:string[],parent?:any){
      this.variable=variable
      this.path=path??[]
      this.parent=parent
  }
  set(value:T){
    this.parent[this.key]=value
  }
  get<K extends keyof T>(name:Extract<K,string>){
      const prop=this.variable?this.variable[name]:undefined
      return new PropertySelector<T[K]>(prop as any,[...this.path,name],this.variable)
  }
  
}
export function TwoWayBinding<TTarget extends NotifyInit>(
  select: (selector:PropertySelector<TTarget>)=>PropertySelector<unknown>,
  converter?: Converter
){
  return function (target: TTarget, propertyKey: string) {
    if (target.initialized == undefined) {
      target.initialized = new EventEmitter();
    }
    target.initialized.subscribe(initializedTarget=>{
      target=initializedTarget
      const selector=select(new PropertySelector<TTarget>(target))
      if(selector.parent==undefined){
        throw Error('parent is undefined')
      }
      const fieldKey = '_' + propertyKey;
      const originalTargetValue = target[propertyKey];
      const originalSourceValue=selector.variable
      let isTargetSetterCalled=false
      if (originalTargetValue == undefined && originalSourceValue != undefined) {
        target[fieldKey] = converter.convert(originalSourceValue);
      } else if (originalTargetValue != undefined) {
        selector.set(converter.convertBack(originalTargetValue))
      }
      const targetEventKey=propertyKey+"Change"
      const targetEvent=target[targetEventKey] as EventEmitter<any>
      const sourceEventKey=selector.key+"Change"
      const sourceEvent=selector.parent[sourceEventKey] as EventEmitter<any>
      sourceEvent.subscribe(v=>{
        if (!isTargetSetterCalled) {
          target[fieldKey] = converter.convert(v);
        }
        targetEvent.emit(target[fieldKey]);
      });

      Object.defineProperty(target, propertyKey, {
        get() {
          return target[fieldKey];
        },
        set(v) {
          isTargetSetterCalled = true;
          const oldValue = target[fieldKey];
          if (oldValue !== v) {
            target[fieldKey] = v;
            selector.set(converter.convertBack(v))
          }
          isTargetSetterCalled = false;
        },
      });
    })
}

}



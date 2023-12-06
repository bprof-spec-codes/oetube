import { Component,forwardRef,ContentChild,AfterViewInit,Input, AfterContentInit } from '@angular/core';
import { ScrollViewComponent, ScrollViewOptions, ScrollViewProviderComponent } from '../scroll-view.component';
import type { EntityDto } from '@abp/ng.core';
import { Observable } from 'rxjs';
import { PaginationDto, QueryDto } from '@proxy/application/dtos';

@Component({
  selector: 'app-scroll-view-selector',
  templateUrl: './scroll-view-selector.component.html',
  styleUrls: ['./scroll-view-selector.component.scss'],
  providers:[{provide:ScrollViewProviderComponent,useExisting:forwardRef(()=>ScrollViewSelectorComponent)}]
})
export class ScrollViewSelectorComponent<TOutputListDto extends EntityDto<string>=EntityDto<string>> 
extends ScrollViewProviderComponent<TOutputListDto> {
    
  @ContentChild(ScrollViewProviderComponent<TOutputListDto>) provider:ScrollViewProviderComponent<TOutputListDto>
  get scrollView(): ScrollViewComponent<TOutputListDto> {
     return this.provider.scrollView 
  }
  @Input() allowSelection=true
  @Input() contextId?:string
  @Input() getInitialSelection:(id:string,args:QueryDto)=>Observable<PaginationDto<TOutputListDto>>;
  
  initThis(){
    this.allowSelection=true
    if(this.contextId&&this.getInitialSelection){
      this.getInitialSelection(this.contextId,{pagination:{skip:0,take:2<<32}}).subscribe(r=>{
        this.selectedDatas=r.items
      })
  }
}
}
import { Component,AfterContentInit,ContentChild,forwardRef, Directive, ContentChildren, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { AppTemplate, TemplateRefCollectionComponent} from 'src/app/template-ref-collection/template-ref-collection.component';




  
@Component({
  selector: 'app-scroll-view-contents',
  templateUrl: './scroll-view-contents.component.html',
  styleUrls: ['./scroll-view-contents.component.scss'],
})
export class ScrollViewContentsComponent implements AfterViewInit{
  @ContentChild(TemplateRefCollectionComponent<ScrollViewContent>) templateCollection:TemplateRefCollectionComponent<ScrollViewContent>

  constructor(private changeDetector:ChangeDetectorRef){
    
  }
  get items(){
    return this.templateCollection.items
  }
  get selectedItem(){
    return this.templateCollection.selectedItem
  }
  
  set selectedItem(v:AppTemplate<ScrollViewContent>){
    this.templateCollection.selectedItem=v
  }

  onSelectionChanged(e)
  {
   this.selectedItem=e.addedItems[0]
  }
ngAfterViewInit(): void {
    this.changeDetector.detectChanges()
}
}

export interface ScrollViewContent{
  key:string
  hint:string
  icon:string
  layoutClassList:string
}

import { Component,ElementRef,forwardRef,TemplateRef,ViewChild, ViewContainerRef } from '@angular/core';
import { SearchItem } from 'src/app/scroll-view/drop-down-search/search-item';
import { TemplateRefCollectionComponent } from 'src/app/template-ref-collection/template-ref-collection.component';

@Component({
  selector: 'app-group-search',
  templateUrl: './group-search.component.html',
  styleUrls: ['./group-search.component.scss'],
  providers:[
    {
      provide:TemplateRefCollectionComponent<SearchItem>,useExisting:forwardRef(()=>GroupSearchComponent)
    }
  ]
})
export class GroupSearchComponent extends TemplateRefCollectionComponent<SearchItem> {

  inputItems:SearchItem[]=[
     new SearchItem().init({key:"name",display:"Name"}),
     new SearchItem().init({key:"creationTime",display:"Creation Time"})
  ]

}

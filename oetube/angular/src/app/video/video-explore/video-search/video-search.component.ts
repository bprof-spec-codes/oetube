import { Component, forwardRef, ViewChild } from '@angular/core';
import { DropDownSearchComponent } from 'src/app/scroll-view/drop-down-search/drop-down-search.component';
import { SearchItem } from 'src/app/scroll-view/drop-down-search/search-item';
import { TemplateRefCollectionComponent } from 'src/app/template-ref-collection/template-ref-collection.component';

@Component({
  selector: 'app-video-search',
  templateUrl: './video-search.component.html',
  styleUrls: ['./video-search.component.scss'],
  providers:[
    {
      provide:TemplateRefCollectionComponent,useExisting:forwardRef(()=>VideoSearchComponent)
    }
  ]
})
export class VideoSearchComponent extends TemplateRefCollectionComponent<SearchItem> {
  inputItems:SearchItem[]=[
    new SearchItem().init({
      key:"name",
      display:"Name",
      allowSorting:true,
    }),
    new SearchItem().init({
      key:"creationTime",
      display:"Creation Time",
      allowSorting:true,
    }),
    new SearchItem().init({
      key:"duration",
      display:"Duration",
      allowSorting:true
    })
  ]
}

import { Component, forwardRef } from '@angular/core';
import { SearchItem } from 'src/app/scroll-view/drop-down-search/search-item';
import { TemplateRefCollectionComponent } from 'src/app/template-ref-collection/template-ref-collection.component';

@Component({
  selector: 'app-playlist-search',
  templateUrl: './playlist-search.component.html',
  styleUrls: ['./playlist-search.component.scss'],
  providers:[
    {
      provide:TemplateRefCollectionComponent,useExisting:forwardRef(()=>PlaylistSearchComponent)
    }
  ]
})
export class PlaylistSearchComponent extends TemplateRefCollectionComponent<SearchItem>{
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
  ]
}

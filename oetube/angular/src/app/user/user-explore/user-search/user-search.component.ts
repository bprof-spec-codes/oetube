import { Component, forwardRef, ViewChild } from '@angular/core';
import { SearchItem } from 'src/app/scroll-view/drop-down-search/search-item';
import { TemplateRefCollectionComponent } from 'src/app/template-ref-collection/template-ref-collection.component';

@Component({
  selector: 'app-user-search',
  templateUrl: './user-search.component.html',
  styleUrls: ['./user-search.component.scss'],
  providers:[
    {
      provide:TemplateRefCollectionComponent,useExisting:forwardRef(()=>UserSearchComponent)
    }
  ]
})
export class UserSearchComponent extends TemplateRefCollectionComponent<SearchItem> {
  inputItems:SearchItem[]=[
    new SearchItem().init({key:"name",display:"Name"}),
    new SearchItem().init({key:"emailDomain",display:"Email Domain"}),
    new SearchItem().init({key:"creationTime",display:"Registration"})
 ]
}

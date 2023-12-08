import { Component, forwardRef, ViewChild } from '@angular/core';
import { TemplateRefCollectionComponent } from 'src/app/template-ref-collection/template-ref-collection.component';
import { DropDownSearchComponent } from '../../drop-down-search/drop-down-search.component';
import { SearchItem } from '../../drop-down-search/search-item';

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

import { Component, forwardRef, ViewChild } from '@angular/core';
import { TemplateRefCollectionComponent } from 'src/app/template-ref-collection/template-ref-collection.component';
import { DropDownSearchComponent } from '../../drop-down-search/drop-down-search.component';
import { SearchItem } from '../../drop-down-search/search-item';

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

  get dropDownSearchComponent(){
    return this._dropDownSearchComponent
  }
  @ViewChild(DropDownSearchComponent) private  _dropDownSearchComponent

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

import { Component, forwardRef, Input, ViewChild } from '@angular/core';
import { ScrollViewContent } from 'src/app/scroll-view/scroll-view-contents/scroll-view-contents.component';
import { TemplateRefCollectionComponent } from 'src/app/template-ref-collection/template-ref-collection.component';

@Component({
  selector: 'app-group-contents',
  templateUrl: './group-contents.component.html',
  styleUrls: ['./group-contents.component.scss'],
  providers:[
    {provide:TemplateRefCollectionComponent,useExisting:forwardRef(()=>GroupContentsComponent)}
  ]
})
export class GroupContentsComponent extends TemplateRefCollectionComponent<ScrollViewContent> {
  inputItems:ScrollViewContent[]=[
    {key:"tile",hint:"Tile",icon:"rowfield",layoutClassList:"d-flex flex-row flex-wrap justify-content-center"},
  ]
  @Input() contextNavigation=false
}

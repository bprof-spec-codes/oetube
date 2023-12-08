import { Component, forwardRef, ViewChild } from '@angular/core';
import { TemplateRefCollectionComponent } from 'src/app/template-ref-collection/template-ref-collection.component';
import {ScrollViewContent, ScrollViewContentsComponent } from '../../scroll-view-contents/scroll-view-contents.component';

@Component({
  selector: 'app-video-contents',
  templateUrl: './video-contents.component.html',
  styleUrls: ['./video-contents.component.scss'],
  providers:[
    {provide:TemplateRefCollectionComponent,useExisting:forwardRef(()=>VideoContentsComponent)}
  ]
})
export class VideoContentsComponent extends TemplateRefCollectionComponent<ScrollViewContent> {
  inputItems:ScrollViewContent[]=[
    {key:"tile",hint:"Tile",icon:"rowfield",layoutClassList:"d-flex flex-row flex-wrap justify-content-center"},
    {key:"list",hint:"List",icon:"fields",layoutClassList:"d-flex flex-row flex-wrap justify-content-center"}
  ]
}

import { Component, forwardRef } from '@angular/core';
import { ScrollViewContent } from 'src/app/scroll-view/scroll-view-contents/scroll-view-contents.component';
import { TemplateRefCollectionComponent } from 'src/app/template-ref-collection/template-ref-collection.component';

@Component({
  selector: 'app-playlist-contents',
  templateUrl: './playlist-contents.component.html',
  styleUrls: ['./playlist-contents.component.scss'],
  providers:[
    {provide:TemplateRefCollectionComponent,useExisting:forwardRef(()=>PlaylistContentsComponent)}
  ]
})
export class PlaylistContentsComponent extends TemplateRefCollectionComponent<ScrollViewContent> {
  inputItems:ScrollViewContent[]=[
    {key:"tile",hint:"Tile",icon:"rowfield",layoutClassList:"d-flex flex-row justify-content-center flex-wrap"},
    {key:"list",hint:"List",icon:"fields",layoutClassList:"d-flex flex-row justify-content-center flex-wrap"}
  ]
}

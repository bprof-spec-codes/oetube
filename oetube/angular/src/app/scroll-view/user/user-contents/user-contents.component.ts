import { Component,ViewChild,forwardRef } from '@angular/core';
import { TemplateRefCollectionComponent } from 'src/app/template-ref-collection/template-ref-collection.component';
import { ScrollViewContent} from '../../scroll-view-contents/scroll-view-contents.component';

@Component({
  selector: 'app-user-contents',
  templateUrl: './user-contents.component.html',
  styleUrls: ['./user-contents.component.scss'],
  providers:[
    {provide:TemplateRefCollectionComponent,useExisting:forwardRef(()=>UserContentsComponent)}
  ]
})
export class UserContentsComponent extends TemplateRefCollectionComponent<ScrollViewContent>{
  inputItems:ScrollViewContent[]=[
    {key:"tile",hint:"Tile",icon:"rowfield",layoutClassList:"d-flex flex-row flex-wrap justify-content-center"},
    {key:"list",hint:"List",icon:"fields",layoutClassList:"d-flex flex-column flex-wrap justify-content-center"}
  ]
}


import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss'],
})
export class GroupComponent {

items:TabItem[]=[
  {title:"explore",loaded:false,visible:true,template:"explore"},
  {title:"create",loaded:false,visible:true,template:"create"}
]
selectedItem:TabItem

onSelectedItemChange(item:TabItem){
  if(!item.loaded){
    item.loaded=true
  }
}
}

type TabItem={title:string,visible:boolean,loaded:boolean,template:string}
import { Component,Input, OnInit } from '@angular/core';
import { GroupListItemDto } from '@proxy/application/dtos/groups';

@Component({
  selector: 'app-group-tile-item',
  templateUrl: './group-tile-item.component.html',
  styleUrls: ['./group-tile-item.component.scss']
})
export class GroupTileItemComponent implements OnInit {
  @Input() item:GroupListItemDto
  @Input() contextNavigation=false
  contextItems=[]
  ngOnInit(): void {
    if(this.contextNavigation){
      this.contextItems=[
        {text:"Open in new tab "+this.item.name, link:["/group",this.item.id]},
      ]
      if(this.item.creator){
        this.contextItems.push(
        {text:"Open in new tab "+this.item.creator.name, link:["/user",this.item.creator.id]}
        )
      }
    }
  }
}

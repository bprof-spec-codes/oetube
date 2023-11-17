import { Component, OnInit } from '@angular/core';
import { VideoQueryDto } from '@proxy/application/dtos/videos';
import { ConfigStateService } from '@abp/ng.core';
@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss']
})
export class GroupComponent implements OnInit {

  constructor(private config:ConfigStateService) {
  
  }
  currentUser=this.config.getOne("currentUser")

  panels=["Explore","Create","My Groups"]
  ngOnInit(): void {
  ;
    
  }

}

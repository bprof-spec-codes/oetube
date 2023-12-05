import { Component } from '@angular/core';
import {ActivatedRoute } from '@angular/router'
@Component({
  selector: 'app-group-details',
  templateUrl: './group-details.component.html',
  styleUrls: ['./group-details.component.scss']
})
export class GroupDetailsComponent {
  constructor(activatedRoute:ActivatedRoute){
    console.log(activatedRoute)
  }
}

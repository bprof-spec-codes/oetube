import { Component, OnInit } from '@angular/core';
import { VideoService } from '@proxy/application';
import { VideoDto } from '@proxy/application/dtos/videos';
import { LazyTabItem } from 'src/app/lazy-tab-panel/lazy-tab-panel.component';
import{ ActivatedRoute, Router } from "@angular/router"
@Component({
  selector: 'app-video-details',
  templateUrl: './video-details.component.html',
  styleUrls: ['./video-details.component.scss']
})
export class VideoDetailsComponent implements OnInit {
  inputItems:LazyTabItem[]=[
    {key:"watch",title:"Watch",authRequired:false,onlyCreator:false,isLoaded:true,visible:true},
    {key:"edit",title:"Edit",authRequired:true,onlyCreator:true,isLoaded:false,visible:true},
  ]
  id:string
  model:VideoDto

  constructor(private videoService:VideoService,private route:ActivatedRoute,private router:Router){
  }
ngOnInit(): void {
    this.route.paramMap.subscribe(p=>{
      this.id=p.get("id")
    this.videoService.get(this.id).subscribe(v=>{
      this.model=v
    })
    })
}
onDeleted(){
  this.router.navigate(["video"])  
}

}

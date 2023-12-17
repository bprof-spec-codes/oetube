import { Component } from '@angular/core';
import { PlaylistService } from '@proxy/application';
import { LazyTabItem } from 'src/app/lazy-tab-panel/lazy-tab-panel.component';
import {ActivatedRoute,Router} from "@angular/router"
import { PlaylistDto } from '@proxy/application/dtos/playlists';
@Component({
  selector: 'app-playlist-details',
  templateUrl: './playlist-details.component.html',
  styleUrls: ['./playlist-details.component.scss']
})
export class PlaylistDetailsComponent {
  inputItems:LazyTabItem[]=[
    {key:"details",title:"Details",authRequired:false,onlyCreator:false,isLoaded:true,visible:true},
    {key:"edit",title:"Edit",authRequired:true,onlyCreator:true,isLoaded:false,visible:true}
  ]
  selectedIndex: number;


  model:PlaylistDto
  constructor(private service:PlaylistService,route:ActivatedRoute,
    private router: Router
    
    ){
    route.paramMap.subscribe(p=>{
      service.get(p.get("id")).subscribe(r=>{
        this.model=r
      })
    })
  }
  onDeleted() {
    this.router.navigate(['/playlist']);
  }
  onSubmitted(v: PlaylistDto) {
    window.location.reload()
  }
}

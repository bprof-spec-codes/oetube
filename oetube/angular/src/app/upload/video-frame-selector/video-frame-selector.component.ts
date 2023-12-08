import { Component,Input,OnInit,OnDestroy,EventEmitter } from '@angular/core';
import { VideoService } from '@proxy/application';
import { VideoIndexImagesDto } from '@proxy/application/dtos/videos';
import { RestService, Rest } from '@abp/ng.core';
import { HttpRequest } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable,lastValueFrom } from 'rxjs';
import { UrlService } from 'src/app/auth/url-service/url.service';
@Component({
  selector: 'app-video-frame-selector',
  templateUrl: './video-frame-selector.component.html',
  styleUrls: ['./video-frame-selector.component.scss']
})
export class VideoFrameSelectorComponent implements OnInit,OnDestroy {

  @Input() itemHeight:number=120
  @Input() itemWidth:number=185
  @Input() height:number=180
  @Input() videoId:string
  dto:VideoIndexImagesDto
  value?:number
  valueChange:EventEmitter<number>=new EventEmitter()
  selected:string
  indexImages:string[]=[]
  styledImages:string[]=[]
  constructor(private urlService:UrlService, private videoService:VideoService) 
  {
  }
  async ngOnInit() {
    if(this.videoId){
      this.dto=await lastValueFrom(this.videoService.getIndexImages(this.videoId))
      this.selected=await lastValueFrom(this.urlService.getResourceUrl(this.dto.selected))
      for (const image of this.dto.indexImages) {
        const resourceUrl=await lastValueFrom(this.urlService.getResourceUrl(image))
        this.indexImages.push(resourceUrl)
      }
    }
   
  }
  onItemClick(e){
    this.value=e.itemIndex+1
    this.valueChange.emit(this.value)
    this.selected=this.indexImages[e.itemIndex]
  }
  ngOnDestroy() {
    URL.revokeObjectURL(this.selected)
    this.indexImages.forEach(i=>{
      URL.revokeObjectURL(i)
    })
    
  }
}


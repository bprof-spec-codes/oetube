import { RestService, Rest } from '@abp/ng.core';
import { environment } from 'src/environments/environment';
import { Observable,map,from,lastValueFrom } from 'rxjs';
import { Injectable,Directive,HostBinding,ElementRef,Renderer2,OnInit,OnDestroy } from '@angular/core';

@Injectable({
    providedIn: 'root',
  })
export class UrlService{
    private static objectUrls:Map<string,string>=new Map()
    constructor(private restService:RestService){
    }
  
    public defaultHost=environment.apis.default.url
    
    getRelativeUrl(absoluteUrl:string){
      return absoluteUrl.replace(this.defaultHost,"")
    }
    getAbsoluteUrl(relativeUrl:string){
      return this.defaultHost+relativeUrl
    }
    getResource(url:string):Observable<Blob>{
     if(url.startsWith("http")){
      url=this.getRelativeUrl(url)
     }
     return this.restService.request<any,Blob>({method:"GET",url:url,responseType:"blob"},{apiName:"Default"})
    }
    getResourceUrl(url:string):Observable<string>{
        return this.getResource(url).pipe(map(b=>URL.createObjectURL(b)))
    }
    getBase64Url(url:string):Observable<string>{
      return from(new Promise<string>(async (resolve,_)=>{
        const blob=await lastValueFrom(this.getResource(url))
        const reader=new FileReader()
        reader.onloadend=()=>resolve(reader.result as string)
        reader.readAsDataURL(blob)
      }))
    }
  }
  


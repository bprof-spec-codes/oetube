import { Component, OnInit } from '@angular/core';
import { GroupService, OeTubeUserService, PlaylistService, VideoService } from '@proxy/application';
import { Observable } from 'rxjs';
import type { EntityDto } from '@abp/ng.core';
@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.scss']
})
export class AdministrationComponent implements OnInit{
  defaultImages:DefaultImage[]=[]
  deleteContents:DeleteContent[]=[]
  selectedDelete:DeleteContent
  id:string
  guidPattern=new RegExp("[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}")
  isValid:boolean=false
  constructor(private groupService:GroupService,private userService:OeTubeUserService, private videoService:VideoService,private playlistService:PlaylistService){
    groupService.getDefaultImage()
  }
  private createDefaultImage(name:string,blob:Blob,uploadMethod:(input)=>Observable<any>):DefaultImage{
    return {name:name,formData:null,objectUrl:URL.createObjectURL(blob),uploadMethod:uploadMethod}
  }
  ngOnInit(): void {
    this.groupService.getDefaultImage().subscribe(r => {
      this.defaultImages.push(this.createDefaultImage("Group",r,this.groupService.uploadDefaultImage))      
      this.userService.getDefaultImage().subscribe(r=>{
        this.defaultImages.push(this.createDefaultImage("User",r,this.userService.uploadDefaultImage))
        this.playlistService.getDefaultImage().subscribe(r=>{
          this.defaultImages.push(this.createDefaultImage("Playlist",r,this.playlistService.uploadDefaultImage))
        })
      })
    })
    this.deleteContents.push({name:"Group",deleteMethod:this.groupService.delete})
    this.deleteContents.push({name:"Playlist",deleteMethod:this.playlistService.delete})
    this.deleteContents.push({name:"Video",deleteMethod:this.videoService.delete})
    this.selectedDelete=this.deleteContents[0]
    ;
  }
  popupVisible:boolean
  popupMessage:string
  imageUpload(defaultImage:DefaultImage){
    if(defaultImage.formData){
      defaultImage.uploadMethod(defaultImage.formData).subscribe(
        {
          next:()=>{
            defaultImage.formData=null
            this.popupVisible=true
            this.popupMessage="The image successfully uploaded!"
          },
          error:(e)=>console.log(e)
        }

      )
    }
  }
  delete(){
    this.selectedDelete.deleteMethod(this.id).subscribe(r=>{
      this.popupVisible=true
      this.popupMessage="The content successfully deleted!"
      this.id=""
    })
  }
  close(){
    this.popupVisible=false
  }
}
type DefaultImage={
  name:string
  objectUrl:string
  formData?:FormData
  uploadMethod:(input:FormData)=>Observable<any>
}
type DeleteContent={
  name:string
  deleteMethod:(id:string)=>Observable<any>
}
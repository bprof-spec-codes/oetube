import { Directive,Output,ElementRef, Input,EventEmitter,HostListener } from '@angular/core';

@Directive(
    {
      selector:"[appDoubleClick]",
      standalone:true
    }
  )
  export class DoubleClickDirective{

    constructor(private elementRef:ElementRef<any>){

    }
    @Output() doubleClick=new EventEmitter<any>()
    @Input() doubleClickEnabled:boolean=true
    
    clickCount:number=0;
    @HostListener("mousedown") mouseDown(e:any){
        if(this.doubleClickEnabled){
            this.clickCount++
            console.log("mousedown")
            setTimeout(()=>{
                if(this.clickCount>1){
                    this.doubleClick.emit(this.elementRef)
                }
                this.clickCount=0
            },250)
        }
    }
  }
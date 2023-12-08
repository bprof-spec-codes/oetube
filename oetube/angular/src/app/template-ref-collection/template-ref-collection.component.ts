import { Component,Directive,ContentChildren,Input,TemplateRef,AfterContentInit,EventEmitter,QueryList, ViewChildren, AfterViewInit} from '@angular/core';
export type TemplateItemContext={ key:string }

@Directive({
  selector:'[appTemplate]',
})
export class AppTemplateDirective{
   _appTemplate:string
  @Input()  set appTemplate(v:string){
    this._appTemplate=v
   }
   get key():string{
      return this._appTemplate
   }
   constructor(public templateRef:TemplateRef<any>){
   }
}
export type AppTemplate<C extends TemplateItemContext=TemplateItemContext>= C & {appTemplate:TemplateRef<any>}

@Component({
  selector: 'app-template-ref-collection',
  template:""
})
export class TemplateRefCollectionComponent<C extends TemplateItemContext=TemplateItemContext> implements AfterViewInit {
  
  @Input() inputItems:C[]=[]
  items:AppTemplate<C>[]=[]
  selectedItem:AppTemplate<C>


  @ViewChildren(AppTemplateDirective) protected templateQuery:QueryList<AppTemplateDirective>
  ngAfterViewInit(): void {
    this.inputItems.forEach(i=>{
      const result=this.templateQuery.find(t=>t.key==i.key)
      if(result){
        const template=i as AppTemplate<C>
        template.appTemplate=result.templateRef
        this.items.push(template)
      }
    })
    if(this.items.length>0){
      this.selectedItem=this.items[0]
    }
  }
}

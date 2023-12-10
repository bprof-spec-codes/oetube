import {
  Component,
  Directive,
  ContentChildren,
  Input,
  TemplateRef,
  Output,
  AfterContentInit,
  EventEmitter,
  QueryList,
  ViewChildren,
  AfterViewInit,
} from '@angular/core';
export type TemplateItemContext = { key: string };

@Directive({
  selector: '[appTemplate]',
})
export class AppTemplateDirective {
  _appTemplate: string;
  @Input() set appTemplate(v: string) {
    this._appTemplate = v;
  }
  get key(): string {
    return this._appTemplate;
  }
  constructor(public templateRef: TemplateRef<any>) {}
}
export type AppTemplate<C extends TemplateItemContext = TemplateItemContext> = C & {
  appTemplate: TemplateRef<any>;
};

@Component({
  selector: 'app-template-ref-collection',
  template: '<ng-content></ng-content>',
})
export class TemplateRefCollectionComponent<C extends TemplateItemContext = TemplateItemContext>
  implements AfterViewInit
{
  @Input() inputItems: C[] = [];
  items: AppTemplate<C>[] = [];

  private _selectedItem: AppTemplate<C>;
  get selectedItem(): AppTemplate<C> {
    return this._selectedItem;
  }
  @Input() set selectedItem(v: AppTemplate<C>) {
    const index = this.items.indexOf(v);
    if (v != this._selectedItem && index != -1) {
      this._selectedItem = v;
      this._selectedIndex = index;
      this.selectedItemChange.emit(this._selectedItem);
      this.selectedIndexChange.emit(this._selectedIndex);
    }
  }
  @Output() selectedItemChange: EventEmitter<AppTemplate<C>> = new EventEmitter();

  private _selectedIndex: number;
  get selectedIndex() {
    return this._selectedIndex;
  }
  @Input() set selectedIndex(v: number) {
    if (v != this._selectedIndex && v >= 0 && v < this.items.length) {
      this._selectedIndex = v;
      this._selectedItem = this.items[v];
      this.selectedItemChange.emit(this._selectedItem);
      this.selectedIndexChange.emit(this._selectedIndex);
    }
  }

  @Output() selectedIndexChange: EventEmitter<number> = new EventEmitter();

  @ContentChildren(AppTemplateDirective)
  protected contentTemplateQuery: QueryList<AppTemplateDirective>;
  @ViewChildren(AppTemplateDirective) protected viewTemplateQuery: QueryList<AppTemplateDirective>;

  protected collectItems(query: QueryList<AppTemplateDirective>) {
    this.inputItems.forEach(i => {
      if (!this.items.find(x => x.key == i.key)) {
        const result = query.find(t => t.key == i.key);
        if (result) {
          const template = i as AppTemplate<C>;
          template.appTemplate = result.templateRef;
          this.items.push(template);
        }
      }
    });
  }
  ngAfterViewInit(): void {
    this.collectItems(this.contentTemplateQuery);
    this.collectItems(this.viewTemplateQuery);
    console.log(this.contentTemplateQuery)
    console.log(this)
    if (this.items.length > 0) {
      this._selectedItem = this.items[0];
    }
  }
}

import {
  Component,
  Input,
  Output,
  EventEmitter,
  AfterContentInit,
  ContentChild,
  forwardRef,
  ViewChild,
  OnInit,
  AfterViewInit,
} from '@angular/core';
import { DxButtonComponent } from 'devextreme-angular';
import { ScrollViewComponent } from '../scroll-view.component';
import type { EntityDto } from '@abp/ng.core';

@Component({
  selector: 'app-scroll-view-selector-popup',
  templateUrl: './scroll-view-selector-popup.component.html',
  styleUrls: ['./scroll-view-selector-popup.component.scss'],
})
export class ScrollViewSelectorPopupComponent<
  TOutputListDto extends EntityDto<string> = EntityDto<string>
>  implements AfterContentInit{

  @ContentChild(ScrollViewComponent<TOutputListDto>) scrollView: ScrollViewComponent<TOutputListDto>;

  @Input() title: string;
  @Input() closeButtonOptions: Partial<DxButtonComponent & any> = {
    icon: 'close',
    type: 'normal',
    stylingMode: 'contained',
    onClick: () => this.close(),
  };

  @Input() value: string[];
  @Output() valueChange: EventEmitter<string[]> = new EventEmitter<string[]>();
  
  @Input() icon: string;
  @Input() text: string;
  @Input() stylingMode: string;
  @Input() disabled: boolean;
  @Input() type: string;

  visible = false;
  isOpened = false;
  show() {
    if (!this.isOpened) {
      this.isOpened = true;
      this.scrollView.dataSource.reload()
    }
    this.visible = true;
  }

  close() {
    this.value = this.scrollView.dataSource.selectedDatas.map(s => s.id);
    this.valueChange.emit(this.value);
    this.scrollView.dataSource.clearCachedData()
    this.visible = false;
  }

  ngAfterContentInit(): void {
    this.scrollView.initialLoad=false
    this.scrollView.dataSource.allowSelection=true   
  }

  mapButton(source: Partial<DxButtonComponent>, destination: DxButtonComponent) {
    if (source) {
      Object.keys(source).forEach(k => {
        debugger;
        const value = source[k];
        if (value) {
          destination[k] = value;
        }
        destination.applyOptions();
      });
    }
  }
  setOptions(): void {

  }
}

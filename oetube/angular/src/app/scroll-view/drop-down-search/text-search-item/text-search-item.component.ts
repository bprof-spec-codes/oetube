import { Component,Input } from '@angular/core';
import { SearchItem } from '../search-item';

@Component({
  selector: 'app-text-search-item',
  templateUrl: './text-search-item.component.html',
  styleUrls: ['./text-search-item.component.scss']
})
export class TextSearchItemComponent {
  @Input() searchItem:SearchItem


}

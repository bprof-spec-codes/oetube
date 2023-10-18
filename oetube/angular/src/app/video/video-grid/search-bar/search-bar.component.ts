import { Component, EventEmitter, Output } from '@angular/core';

import { ClickEvent } from 'devextreme/ui/button';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss'],
})
export class SearchBarComponent {
  @Output() searchClicked = new EventEmitter<string>();

  public searchPhrase: string;

  searchButtonOptions = {
    icon: 'search',
    disabled: false,
    visible: true,
    stylingMode: 'text',
    onClick: () => this.onSearch(),
  };

  onSearch() {
    this.searchClicked.emit(this.searchPhrase);
  }
}

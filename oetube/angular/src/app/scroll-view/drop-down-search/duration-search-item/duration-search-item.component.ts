import { Component, Input, OnInit } from '@angular/core';
import { debug } from 'console';
import { Converter } from 'src/app/base-types/converter';
import { Time, TimeInitType } from 'src/app/base-types/time';
import { SearchItem } from '../search-item';

@Component({
  selector: 'app-duration-search-item',
  templateUrl: './duration-search-item.component.html',
  styleUrls: ['./duration-search-item.component.scss'],
})
export class DurationSearchItemComponent implements OnInit {
  @Input() searchItem: SearchItem;

  _exRangeMin: Time = Time.zero;
  get exRangeMin(): number {
    return this._exRangeMin.totalSeconds;
  }
  @Input() set exRangeMin(v: TimeInitType) {
    this._exRangeMin = Time.from(v);
  }
  _exRangeMax: Time = Time.from('04:00:00');
  get exRangeMax(): number {
    return this._exRangeMax.totalSeconds;
  }
  @Input() set exRangeMax(v: TimeInitType) {
    this._exRangeMax = Time.from(v);
  }

  @Input() color: string = '#1D294D';

  protected defaultValueKeys?: string[] = ['Min', 'Max'];
  protected defaultConverter: Converter<any, any> = {
    convert: v => {
      const minkey = 'durationMin';
      const maxkey = 'durationMax';
      const min = v[minkey];
      const max = v[maxkey];
      const converted = {};
      converted[minkey] =
        min && min > this.exRangeMin ? new Time({ seconds: min }).toString() : undefined;
      converted[maxkey] =
        max && max < this.exRangeMax ? new Time({ seconds: max }).toString() : undefined;
      return converted;
    },
  };
  customizeText(v) {
      return new Time({ seconds: v.value }).toString();
  }

  ngOnInit(): void {
    this.searchItem.converter ??= this.defaultConverter;
    this.searchItem.valueKeys ??= this.defaultValueKeys;
    this.searchItem.init(this.searchItem);
  }
}

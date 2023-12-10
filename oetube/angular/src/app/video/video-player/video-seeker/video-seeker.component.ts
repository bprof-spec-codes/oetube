import { Component, EventEmitter, Input, Output } from '@angular/core';

import { ValueChangedEvent } from 'devextreme/ui/progress_bar';

@Component({
  selector: 'app-video-seeker',
  templateUrl: './video-seeker.component.html',
  styleUrls: ['./video-seeker.component.scss'],
})
export class VideoSeekerComponent {
  @Input() max = 0;
  @Input() value = 0;
  @Input() label = '';
  @Output() valueChange = new EventEmitter<ValueChangedEvent>();

  changeHandler(event: ValueChangedEvent) {
    this.valueChange.emit(event);
  }
}

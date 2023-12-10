import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-play-control',
  templateUrl: './play-control.component.html',
  styleUrls: ['./play-control.component.scss'],
})
export class PlayControlComponent {
  @Input() icon = 'Play';
  @Input() title = 'play_arrow';
}

import { BehaviorSubject, Observable } from 'rxjs';

import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class VolumeService {
  private volume = new BehaviorSubject<number>(1);

  setVolumeValue(value: number): void {
    this.volume.next(value);
  }

  get volumeValue$(): Observable<number> {
    return this.volume.asObservable();
  }
}

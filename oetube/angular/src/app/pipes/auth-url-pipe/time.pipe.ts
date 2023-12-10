
import { Pipe, PipeTransform,Injectable } from '@angular/core';
import { Time } from '../../base-types/time';



 @Injectable( {providedIn: 'root'
}) @Pipe({ name: 'time', pure: false })
export class TimePipe implements PipeTransform{

    transform(value: string, ...args: any[]) {
        return value.split('.')[0]
    }
}
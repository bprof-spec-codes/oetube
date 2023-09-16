import { Injectable } from '@angular/core';
import{map}from'rxjs/operators';
import { ExampleService } from '@proxy/services';

@Injectable({
  providedIn: 'root'
})
export class DevExtremeService {
  examples$=this.service.getList();

  constructor(private service:ExampleService) 
  { }
}

import { Injectable } from '@angular/core';
import { ValidationService } from '@proxy/application';
import { ValidationsDto } from '@proxy/application/dtos/validations';

@Injectable({
  providedIn: 'root'
})
export class ValidationStoreService {

  private _validations:ValidationsDto

  get validations(){
    return this._validations
  }
  constructor(private validationService:ValidationService) { 
    validationService.get().subscribe(r=>{
      this._validations=r;console.log(r)}
      )
  }
}

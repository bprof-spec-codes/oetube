import type { ValidationsDto } from './dtos/validations/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ValidationService {
  apiName = 'Default';
  

  get = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ValidationsDto>({
      method: 'GET',
      url: '/api/app/validation',
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

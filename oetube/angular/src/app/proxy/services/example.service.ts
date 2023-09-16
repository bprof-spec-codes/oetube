import type { ExampleDto } from './dtos/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ExampleService {
  apiName = 'Default';
  

  create = (name: string, description: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExampleDto>({
      method: 'POST',
      url: '/api/app/example',
      params: { name, description },
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/example/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExampleDto[]>({
      method: 'GET',
      url: '/api/app/example',
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

import type { OeTubeUserDto, OeTubeUserItemDto, UpdateOeTubeUserDto } from './dtos/oe-tube-users/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class OeTubeUserService {
  apiName = 'Default';
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, OeTubeUserDto>({
      method: 'GET',
      url: `/api/app/oe-tube-user/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<OeTubeUserItemDto>>({
      method: 'GET',
      url: '/api/app/oe-tube-user',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateOeTubeUserDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UpdateOeTubeUserDto>({
      method: 'PUT',
      url: `/api/app/oe-tube-user/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

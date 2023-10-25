import type { CreateUpdateGroupDto, GroupDto, GroupItemDto, ModifyEmailDomainsDto, ModifyMembersDto } from './dtos/groups/models';
import type { OeTubeUserItemDto } from './dtos/oe-tube-users/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class GroupService {
  apiName = 'Default';
  

  create = (input: CreateUpdateGroupDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GroupDto>({
      method: 'POST',
      url: '/api/app/group',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/group/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GroupDto>({
      method: 'GET',
      url: `/api/app/group/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getGroupMembers = (id: string, input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<OeTubeUserItemDto>>({
      method: 'GET',
      url: `/api/app/group/${id}/group-members`,
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<GroupItemDto>>({
      method: 'GET',
      url: '/api/app/group',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateGroupDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GroupDto>({
      method: 'PUT',
      url: `/api/app/group/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  updateEmailDomains = (id: string, input: ModifyEmailDomainsDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/group/${id}/email-domains`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  updateMembers = (id: string, input: ModifyMembersDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'PUT',
      url: `/api/app/group/${id}/members`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

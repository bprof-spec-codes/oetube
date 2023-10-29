import type { CreateUpdateGroupDto, GroupDto, GroupListItemDto, GroupQueryDto, ModifyEmailDomainsDto, ModifyMembersDto } from './dtos/groups/models';
import type { UserListItemDto, UserQueryDto } from './dtos/oe-tube-users/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
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
  

  getGroupMembers = (id: string, input: UserQueryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<UserListItemDto>>({
      method: 'GET',
      url: `/api/app/group/${id}/group-members`,
      params: { name: input.name, emailDomain: input.emailDomain, creationTimeMin: input.creationTimeMin, creationTimeMax: input.creationTimeMax, skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName,...config });
  

  getImage = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/app/group/${id}/image`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GroupQueryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<GroupListItemDto>>({
      method: 'GET',
      url: '/api/app/group',
      params: { name: input.name, creationTimeMin: input.creationTimeMin, creationTimeMax: input.creationTimeMax, skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
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
    this.restService.request<any, GroupDto>({
      method: 'PUT',
      url: `/api/app/group/${id}/email-domains`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  updateMembers = (id: string, input: ModifyMembersDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GroupDto>({
      method: 'PUT',
      url: `/api/app/group/${id}/members`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  uploadImage = (id: string, input: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/group/${id}/upload-image`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

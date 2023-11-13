import type { CreateUpdateGroupDto, GroupDto, GroupListItemDto, GroupQueryDto } from './dtos/groups/models';
import type { PaginationDto } from './dtos/models';
import type { UserListItemDto, UserQueryDto } from './dtos/oe-tube-users/models';
import { RestService, Rest } from '@abp/ng.core';
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
      params: { name: input.name, description: input.description, emailDomains: input.emailDomains, members: input.members },
      body: input.image,
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
    this.restService.request<any, PaginationDto<UserListItemDto>>({
      method: 'GET',
      url: `/api/app/group/${id}/group-members`,
      params: { name: input.name, emailDomain: input.emailDomain, creationTimeMin: input.creationTimeMin, creationTimeMax: input.creationTimeMax, itemPerPage: input.itemPerPage, page: input.page, sorting: input.sorting },
    },
    { apiName: this.apiName,...config });
  

  getImage = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/src/group/${id}/image`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: GroupQueryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PaginationDto<GroupListItemDto>>({
      method: 'GET',
      url: '/api/app/group',
      params: { name: input.name, creationTimeMin: input.creationTimeMin, creationTimeMax: input.creationTimeMax, creatorId: input.creatorId, itemPerPage: input.itemPerPage, page: input.page, sorting: input.sorting },
    },
    { apiName: this.apiName,...config });
  

  getThumbnailImage = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/src/group/${id}/thumbnail-image`,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateGroupDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GroupDto>({
      method: 'PUT',
      url: `/api/app/group/${id}`,
      params: { name: input.name, description: input.description, emailDomains: input.emailDomains, members: input.members },
      body: input.image,
    },
    { apiName: this.apiName,...config });
  

  uploadDefaultImage = (input: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/group/upload-default-image',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

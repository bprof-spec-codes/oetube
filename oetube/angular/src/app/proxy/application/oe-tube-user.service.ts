import type { GroupListItemDto, GroupQueryDto } from './dtos/groups/models';
import type { UpdateUserDto, UserDto, UserListItemDto, UserQueryDto } from './dtos/oe-tube-users/models';
import type { VideoListItemDto, VideoQueryDto } from './dtos/videos/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class OeTubeUserService {
  apiName = 'Default';
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserDto>({
      method: 'GET',
      url: `/api/app/oe-tube-user/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getAvaliableVideos = (id: string, input: VideoQueryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<VideoListItemDto>>({
      method: 'GET',
      url: `/api/app/oe-tube-user/${id}/avaliable-videos`,
      params: { name: input.name, creationTimeMin: input.creationTimeMin, creationTimeMax: input.creationTimeMax, durationMin: input.durationMin, durationMax: input.durationMax, skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName,...config });
  

  getCreatedGroups = (id: string, input: GroupQueryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<GroupListItemDto>>({
      method: 'GET',
      url: `/api/app/oe-tube-user/${id}/created-groups`,
      params: { name: input.name, creationTimeMin: input.creationTimeMin, creationTimeMax: input.creationTimeMax, skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName,...config });
  

  getCreatedVideos = (id: string, input: VideoQueryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<VideoListItemDto>>({
      method: 'GET',
      url: `/api/app/oe-tube-user/${id}/created-videos`,
      params: { name: input.name, creationTimeMin: input.creationTimeMin, creationTimeMax: input.creationTimeMax, durationMin: input.durationMin, durationMax: input.durationMax, skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName,...config });
  

  getImage = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/app/oe-tube-user/${id}/image`,
    },
    { apiName: this.apiName,...config });
  

  getJoinedGroups = (id: string, input: GroupQueryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<GroupListItemDto>>({
      method: 'GET',
      url: `/api/app/oe-tube-user/${id}/joined-groups`,
      params: { name: input.name, creationTimeMin: input.creationTimeMin, creationTimeMax: input.creationTimeMax, skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName,...config });
  

  getList = (input: UserQueryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<UserListItemDto>>({
      method: 'GET',
      url: '/api/app/oe-tube-user',
      params: { name: input.name, emailDomain: input.emailDomain, creationTimeMin: input.creationTimeMin, creationTimeMax: input.creationTimeMax, skipCount: input.skipCount, maxResultCount: input.maxResultCount, sorting: input.sorting },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateUserDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserDto>({
      method: 'PUT',
      url: `/api/app/oe-tube-user/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  uploadImage = (id: string, input: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/oe-tube-user/${id}/upload-image`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

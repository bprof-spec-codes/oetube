import type { GroupListItemDto, GroupQueryDto } from './dtos/groups/models';
import type { PaginationDto } from './dtos/models';
import type { StartVideoUploadDto, UpdateVideoDto, VideoDto, VideoIndexImagesDto, VideoListItemDto, VideoQueryDto, VideoUploadStateDto } from './dtos/videos/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class VideoService {
  apiName = 'Default';
  

  continueUpload = (id: string, input: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, VideoUploadStateDto>({
      method: 'POST',
      url: `/api/app/video/${id}/continue-upload`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/video/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, VideoDto>({
      method: 'GET',
      url: `/api/app/video/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getAccessGroups = (id: string, input: GroupQueryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PaginationDto<GroupListItemDto>>({
      method: 'GET',
      url: `/api/app/video/${id}/access-groups`,
      params: { name: input.name, creationTimeMin: input.creationTimeMin, creationTimeMax: input.creationTimeMax, itemPerPage: input.itemPerPage, page: input.page, sorting: input.sorting },
    },
    { apiName: this.apiName,...config });
  

  getHlsList = (id: string, width: number, height: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/src/video/${id}/${width}x${height}/list.m3u8`,
    },
    { apiName: this.apiName,...config });
  

  getHlsSegment = (id: string, width: number, height: number, segment: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/src/video/${id}/${width}x${height}/${segment}.ts`,
    },
    { apiName: this.apiName,...config });
  

  getIndexImage = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/src/video/${id}/index_image`,
    },
    { apiName: this.apiName,...config });
  

  getIndexImageByIndex = (id: string, index: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/src/video/${id}/index_image/${index}`,
    },
    { apiName: this.apiName,...config });
  

  getIndexImages = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, VideoIndexImagesDto>({
      method: 'GET',
      url: `/api/app/video/${id}/index-images`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: VideoQueryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PaginationDto<VideoListItemDto>>({
      method: 'GET',
      url: '/api/app/video',
      params: { name: input.name, creationTimeMin: input.creationTimeMin, creationTimeMax: input.creationTimeMax, durationMin: input.durationMin, durationMax: input.durationMax, itemPerPage: input.itemPerPage, page: input.page, sorting: input.sorting },
    },
    { apiName: this.apiName,...config });
  

  selectIndexImage = (id: string, index: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: `/api/app/video/${id}/select-index-image`,
      params: { index },
    },
    { apiName: this.apiName,...config });
  

  startUpload = (input: StartVideoUploadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, VideoUploadStateDto>({
      method: 'POST',
      url: '/api/app/video/start-upload',
      params: { name: input.name, description: input.description, access: input.access, accessGroups: input.accessGroups },
      body: input.content,
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: UpdateVideoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, VideoDto>({
      method: 'PUT',
      url: `/api/app/video/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

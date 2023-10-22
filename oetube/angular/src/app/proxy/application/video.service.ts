import type { StartVideoUploadDto, VideoDto, VideoItemDto, VideoUploadStateDto } from './dtos/videos/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
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
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, VideoDto>({
      method: 'GET',
      url: `/api/app/video/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getHlsList = (id: string, width: number, height: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/app/video/${id}/${width}x${height}/list.m3u8`,
    },
    { apiName: this.apiName,...config });
  

  getHlsSegment = (id: string, width: number, height: number, segment: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/app/video/${id}/${width}x${height}/${segment}.ts`,
    },
    { apiName: this.apiName,...config });
  

  getIndexImage = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/app/video/${id}/index_image`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<VideoItemDto>>({
      method: 'GET',
      url: '/api/app/video',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  startUpload = (input: StartVideoUploadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, VideoUploadStateDto>({
      method: 'POST',
      url: '/api/app/video/start-upload',
      params: { name: input.name, description: input.description },
      body: input.content,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

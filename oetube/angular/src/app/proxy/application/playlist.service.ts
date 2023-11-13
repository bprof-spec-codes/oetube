import type { PaginationDto } from './dtos/models';
import type { CreateUpdatePlaylistDto, PlaylistDto, PlaylistItemDto, PlaylistQueryDto } from './dtos/playlists/models';
import type { VideoListItemDto, VideoQueryDto } from './dtos/videos/models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PlaylistService {
  apiName = 'Default';
  

  create = (input: CreateUpdatePlaylistDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PlaylistDto>({
      method: 'POST',
      url: '/api/app/playlist',
      params: { name: input.name, description: input.description, items: input.items },
      body: input.image,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/playlist/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PlaylistDto>({
      method: 'GET',
      url: `/api/app/playlist/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getImage = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/src/playlist/${id}/image`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PlaylistQueryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PaginationDto<PlaylistItemDto>>({
      method: 'GET',
      url: '/api/app/playlist',
      params: { name: input.name, creationTimeMin: input.creationTimeMin, creationTimeMax: input.creationTimeMax, creatorId: input.creatorId, itemPerPage: input.itemPerPage, page: input.page, sorting: input.sorting },
    },
    { apiName: this.apiName,...config });
  

  getThumbnailImage = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>({
      method: 'GET',
      responseType: 'blob',
      url: `/api/src/playlist/${id}/thumbnail-image`,
    },
    { apiName: this.apiName,...config });
  

  getVideos = (id: string, input: VideoQueryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PaginationDto<VideoListItemDto>>({
      method: 'GET',
      url: `/api/app/playlist/${id}/videos`,
      params: { name: input.name, creationTimeMin: input.creationTimeMin, creationTimeMax: input.creationTimeMax, durationMin: input.durationMin, durationMax: input.durationMax, creatorId: input.creatorId, itemPerPage: input.itemPerPage, page: input.page, sorting: input.sorting },
    },
    { apiName: this.apiName,...config });
  

  uploadDefaultImage = (input: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'POST',
      url: '/api/app/playlist/upload-default-image',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

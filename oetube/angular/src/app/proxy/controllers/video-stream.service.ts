import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { FileStreamResult } from '../microsoft/asp-net-core/mvc/models';

@Injectable({
  providedIn: 'root',
})
export class VideoStreamService {
  apiName = 'Default';
  

  getM3U8 = (id: string, height: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileStreamResult>({
      method: 'GET',
      url: `/api/app/video/src/${id}/${height}/list.m3u8`,
    },
    { apiName: this.apiName,...config });
  

  getM3U8Segment = (id: string, height: number, segment: number, config?: Partial<Rest.Config>) =>
    this.restService.request<any, FileStreamResult>({
      method: 'GET',
      url: `/api/app/video/src/${id}/${height}/${segment}.ts`,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

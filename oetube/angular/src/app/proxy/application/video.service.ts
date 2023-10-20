import type { StartVideoUploadDto, VideoUploadStateDto } from './dtos/videos/models';
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

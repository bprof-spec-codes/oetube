import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class VideoService {
  apiName = 'Default';
  

  uploadVideo = (content: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'POST',
      responseType: 'text',
      url: '/api/app/video/upload-video',
      body: content,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}

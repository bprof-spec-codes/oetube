import type { Resolution } from '../../../domain/entities/videos/models';
import type { IRemoteStreamContent } from '../../../volo/abp/content/models';
import type { UploadTask } from '../../../infrastructure/ff/models';

export interface ResolutionSrcDto {
  resolution: Resolution;
  src?: string;
}

export interface StartVideoUploadDto {
  name?: string;
  description?: string;
  content: FormData;
}

export interface VideoDto {
  id?: string;
  resolutionsSrc: ResolutionSrcDto[];
  indexImageSrc?: string;
  name?: string;
  description?: string;
  creatorId?: string;
  creationTime?: string;
  duration?: string;
  accessGroups: string[];
  playlistId?: string;
  isUploadCompleted: boolean;
}

export interface VideoFilterDto {
  name?: string;
}

export interface VideoItemDto {
  id?: string;
  name?: string;
  indexImageSrc?: string;
  duration?: string;
  creationTime?: string;
  creatorId?: string;
  playlistId?: string;
}

export interface VideoUploadStateDto {
  id?: string;
  isCompleted: boolean;
  outputFormat?: string;
  remainingTasks: UploadTask[];
}

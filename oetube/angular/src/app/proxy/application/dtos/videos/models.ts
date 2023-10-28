import type { Resolution } from '../../../domain/entities/videos/models';
import type { AccessType } from '../../../domain/entities/videos/access-type.enum';
import type { IRemoteStreamContent } from '../../../volo/abp/content/models';
import type { UploadTask } from '../../../domain/infrastructure/ffmpeg/models';

export interface HlsSourceDto {
  resolution: Resolution;
  src?: string;
}

export interface StartVideoUploadDto {
  name?: string;
  description?: string;
  access: AccessType;
  content: FormData;
}

export interface UpdateAccessGroupsDto {
  accessGroups: string[];
}

export interface UpdateVideoDto {
  name?: string;
  description?: string;
  access: AccessType;
}

export interface VideoDto {
  id?: string;
  hlsSources: HlsSourceDto[];
  indexImageSource?: string;
  name?: string;
  description?: string;
  creatorId?: string;
  creationTime?: string;
  duration?: string;
  accessGroups: string[];
  playlistId?: string;
  isUploadCompleted: boolean;
}

export interface VideoListItemDto {
  id?: string;
  name?: string;
  indexImageSource?: string;
  duration?: string;
  creationTime?: string;
  creatorId?: string;
  playlistId?: string;
}

export interface VideoQueryDto {
  name?: string;
  creationTimeMin?: string;
  creationTimeMax?: string;
  durationMin?: string;
  durationMax?: string;
  skipCount?: number;
  maxResultCount?: number;
  sorting?: string;
}

export interface VideoUploadStateDto {
  id?: string;
  isCompleted: boolean;
  outputFormat?: string;
  remainingTasks: UploadTask[];
}

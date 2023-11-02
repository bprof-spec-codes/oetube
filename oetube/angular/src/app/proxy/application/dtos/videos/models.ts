import type { AccessType } from '../../../domain/entities/videos/access-type.enum';
import type { IRemoteStreamContent } from '../../../volo/abp/content/models';
import type { EntityDto } from '@abp/ng.core';
import type { CreatorDto } from '../oe-tube-users/models';

export interface HlsResolutionDto {
  width: number;
  height: number;
  hlsList?: string;
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

export interface UploadTaskDto {
  width: number;
  height: number;
  arguments?: string;
}

export interface VideoDto extends EntityDto<string> {
  hlsResolutions: HlsResolutionDto[];
  indexImage?: string;
  name?: string;
  description?: string;
  creationTime?: string;
  duration?: string;
  accessGroups: string[];
  playlistId?: string;
  isUploadCompleted: boolean;
  creator: CreatorDto;
}

export interface VideoIndexImagesDto extends EntityDto<string> {
  indexImages: string[];
}

export interface VideoListItemDto extends EntityDto<string> {
  name?: string;
  indexImage?: string;
  duration?: string;
  creationTime?: string;
  playlistId?: string;
  creator: CreatorDto;
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
  remainingTasks: UploadTaskDto[];
}

import type { IRemoteStreamContent } from '../../../volo/abp/content/models';
import type { AccessType } from '../../../domain/entities/videos/access-type.enum';
import type { WebAssemblyState } from './web-assembly-state.enum';
import type { EntityDto } from '@abp/ng.core';
import type { CreatorDto } from '../oe-tube-users/models';
import type { QueryDto } from '../models';

export interface ContinueVideoUploadDto {
  content: FormData;
}

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
  accessGroups: string[];
  webAssemblyState: WebAssemblyState;
}

export interface UpdateVideoDto {
  name?: string;
  description?: string;
  access: AccessType;
  accessGroups: string[];
  indexImage?: number;
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
  access: AccessType;
  playlistId?: string;
  creator: CreatorDto;
}

export interface VideoIndexImagesDto extends EntityDto<string> {
  indexImages: string[];
  selected?: string;
}

export interface VideoListItemDto extends EntityDto<string> {
  name?: string;
  indexImage?: string;
  duration?: string;
  creationTime?: string;
  playlistId?: string;
  access: AccessType;
  creator: CreatorDto;
}

export interface VideoQueryDto extends QueryDto {
  name?: string;
  creationTimeMin?: string;
  creationTimeMax?: string;
  durationMin?: string;
  durationMax?: string;
  creatorId?: string;
}

export interface VideoUploadStateDto {
  id?: string;
  isCompleted: boolean;
  outputFormat?: string;
  remainingTasks: UploadTaskDto[];
}

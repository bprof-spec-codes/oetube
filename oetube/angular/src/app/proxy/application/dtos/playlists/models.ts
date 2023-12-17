import type { IRemoteStreamContent } from '../../../volo/abp/content/models';
import type { EntityDto } from '@abp/ng.core';
import type { CreatorDto } from '../oe-tube-users/models';
import type { QueryDto } from '../models';

export interface CreateUpdatePlaylistDto {
  name: string;
  description?: string;
  items: string[];
  image: FormData;
}

export interface PlaylistDto extends EntityDto<string> {
  name?: string;
  description?: string;
  creationTime?: string;
  items: string[];
  image?: string;
  creator: CreatorDto;
  totalDuration?: string;
}

export interface PlaylistItemDto extends EntityDto<string> {
  name?: string;
  description?: string;
  creationTime?: string;
  thumbnailImage?: string;
  creator: CreatorDto;
  totalDuration?: string;
  itemsCount: number;
}

export interface PlaylistQueryDto extends QueryDto {
  name?: string;
  creationTimeMin?: string;
  creationTimeMax?: string;
  creatorId?: string;
}

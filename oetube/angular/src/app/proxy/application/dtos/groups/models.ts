import type { IRemoteStreamContent } from '../../../volo/abp/content/models';
import type { EntityDto } from '@abp/ng.core';
import type { CreatorDto } from '../oe-tube-users/models';
import type { QueryDto } from '../models';

export interface CreateUpdateGroupDto {
  name: string;
  description?: string;
  emailDomains: string[];
  members: string[];
  image: FormData;
}

export interface GroupDto extends EntityDto<string> {
  name?: string;
  description?: string;
  creationTime?: string;
  emailDomains: string[];
  members: string[];
  image?: string;
  creator: CreatorDto;
  currentUserIsMember: boolean;
  totalMembersCount: number;
}

export interface GroupListItemDto extends EntityDto<string> {
  name?: string;
  creationTime?: string;
  thumbnailImage?: string;
  creator: CreatorDto;
  currentUserIsMember: boolean;
  totalMembersCount: number;
}

export interface GroupQueryDto extends QueryDto {
  name?: string;
  creationTimeMin?: string;
  creationTimeMax?: string;
  creatorId?: string;
}

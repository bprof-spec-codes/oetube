import type { EntityDto } from '@abp/ng.core';
import type { CreatorDto } from '../oe-tube-users/models';

export interface CreateUpdateGroupDto {
  name: string;
  description?: string;
  emailDomains: string[];
  members: string[];
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

export interface GroupQueryDto {
  name?: string;
  creationTimeMin?: string;
  creationTimeMax?: string;
  itemPerPage?: number;
  page?: number;
  sorting?: string;
}

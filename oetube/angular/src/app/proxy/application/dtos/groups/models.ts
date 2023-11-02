import type { EntityDto } from '@abp/ng.core';
import type { CreatorDto } from '../oe-tube-users/models';

export interface CreateUpdateGroupDto {
  name: string;
  description?: string;
}

export interface GroupDto extends EntityDto<string> {
  name?: string;
  description?: string;
  creationTime?: string;
  emailDomains: string[];
  members: string[];
  image?: string;
  creator: CreatorDto;
}

export interface GroupListItemDto extends EntityDto<string> {
  name?: string;
  creationTime?: string;
  thumbnailImage?: string;
  creator: CreatorDto;
}

export interface GroupQueryDto {
  name?: string;
  creationTimeMin?: string;
  creationTimeMax?: string;
  skipCount?: number;
  maxResultCount?: number;
  sorting?: string;
}

export interface ModifyEmailDomainsDto {
  emailDomains: string[];
}

export interface ModifyMembersDto {
  members: string[];
}

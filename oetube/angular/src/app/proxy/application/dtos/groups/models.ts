import type { EntityDto } from '@abp/ng.core';

export interface CreateUpdateGroupDto {
  name: string;
  description?: string;
}

export interface GroupDto extends EntityDto<string> {
  name?: string;
  description?: string;
  creationTime?: string;
  creatorId?: string;
  emailDomains: string[];
  members: string[];
  imageSource?: string;
}

export interface GroupListItemDto extends EntityDto<string> {
  name?: string;
  creationTime?: string;
  creatorId?: string;
  imageSource?: string;
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

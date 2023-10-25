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
}

export interface GroupItemDto extends EntityDto<string> {
  name?: string;
  creationTime?: string;
  creatorId?: string;
}

export interface ModifyEmailDomainsDto {
  emailDomains: string[];
}

export interface ModifyMembersDto {
  members: string[];
}

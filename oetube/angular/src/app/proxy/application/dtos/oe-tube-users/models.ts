import type { EntityDto } from '@abp/ng.core';
import type { IRemoteStreamContent } from '../../../volo/abp/content/models';
import type { QueryDto } from '../models';

export interface CreatorDto extends EntityDto<string> {
  name?: string;
  thumbnailImage?: string;
  currentUserIsCreator: boolean;
}

export interface UpdateUserDto {
  name: string;
  aboutMe?: string;
  image: FormData;
}

export interface UserDto extends EntityDto<string> {
  name?: string;
  aboutMe?: string;
  emailDomain?: string;
  creationTime?: string;
  image?: string;
}

export interface UserListItemDto extends EntityDto<string> {
  name?: string;
  creationTime?: string;
  emailDomain?: string;
  thumbnailImage?: string;
}

export interface UserQueryDto extends QueryDto {
  name?: string;
  emailDomain?: string;
  creationTimeMin?: string;
  creationTimeMax?: string;
}

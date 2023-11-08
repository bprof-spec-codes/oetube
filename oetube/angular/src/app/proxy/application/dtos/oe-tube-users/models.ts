import type { EntityDto } from '@abp/ng.core';

export interface CreatorDto extends EntityDto<string> {
  name?: string;
  thumbnailImage?: string;
  currentUserIsCreator: boolean;
}

export interface UpdateUserDto {
  name: string;
  aboutMe?: string;
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
  thumbnailImageSource?: string;
}

export interface UserQueryDto {
  name?: string;
  emailDomain?: string;
  creationTimeMin?: string;
  creationTimeMax?: string;
  itemPerPage?: number;
  page?: number;
  sorting?: string;
}

import type { EntityDto } from '@abp/ng.core';

export interface CreatorDto extends EntityDto<string> {
  name?: string;
  thumbnailImage?: string;
}

export interface UpdateUserDto {
  name: string;
  aboutMe?: string;
}

export interface UserDto extends EntityDto<string> {
  name?: string;
  aboutMe?: string;
  emailDomain?: string;
  registrationDate?: string;
  image?: string;
}

export interface UserListItemDto extends EntityDto<string> {
  name?: string;
  registrationDate?: string;
  emailDomain?: string;
  thumbnailImageSource?: string;
}

export interface UserQueryDto {
  name?: string;
  emailDomain?: string;
  creationTimeMin?: string;
  creationTimeMax?: string;
  skipCount?: number;
  maxResultCount?: number;
  sorting?: string;
}

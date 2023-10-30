import type { EntityDto } from '@abp/ng.core';

export interface UpdateUserDto {
  name: string;
  aboutMe?: string;
}

export interface UserDto extends EntityDto<string> {
  name?: string;
  aboutMe?: string;
  emailDomain?: string;
  registrationDate?: string;
  imageSource?: string;
}

export interface UserListItemDto extends EntityDto<string> {
  name?: string;
  registrationDate?: string;
  emailDomain?: string;
  imageSource?: string;
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

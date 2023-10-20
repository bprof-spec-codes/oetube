import type { EntityDto } from '@abp/ng.core';

export interface OeTubeUserDto extends EntityDto<string> {
  name?: string;
  aboutMe?: string;
  emailDomain?: string;
  registrationDate?: string;
}

export interface OeTubeUserItemDto extends EntityDto<string> {
  name?: string;
  registrationDate?: string;
  emailDomain?: string;
}

export interface UpdateOeTubeUserDto {
  name: string;
  aboutMe?: string;
}

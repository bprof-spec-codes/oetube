import { mapEnumToOptions } from '@abp/ng.core';

export enum AccessType {
  Private = 0,
  Public = 1,
  Group = 2,
}

export const accessTypeOptions = mapEnumToOptions(AccessType);

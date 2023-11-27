import { mapEnumToOptions } from '@abp/ng.core';

export enum ItemPerPage {
  P10 = 10,
  P20 = 20,
  P50 = 50,
  P100 = 100,
}

export const itemPerPageOptions = mapEnumToOptions(ItemPerPage);

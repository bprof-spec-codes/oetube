import { mapEnumToOptions } from '@abp/ng.core';

export enum WebAssemblyState {
  Disabled = 0,
  Enabled = 1,
}

export const webAssemblyStateOptions = mapEnumToOptions(WebAssemblyState);

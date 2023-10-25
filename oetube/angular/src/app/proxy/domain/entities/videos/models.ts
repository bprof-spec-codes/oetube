import type { ValueObject } from '../../../volo/abp/domain/values/models';

export interface Resolution extends ValueObject {
  width: number;
  height: number;
}

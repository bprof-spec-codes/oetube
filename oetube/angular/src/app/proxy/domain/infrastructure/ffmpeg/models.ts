import type { Resolution } from '../../entities/videos/models';

export interface UploadTask {
  resolution: Resolution;
  arguments?: string;
}

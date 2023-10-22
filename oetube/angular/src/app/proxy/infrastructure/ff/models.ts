import type { Resolution } from '../../domain/entities/videos/models';

export interface UploadTask {
  resolution: Resolution;
  arguments?: string;
}

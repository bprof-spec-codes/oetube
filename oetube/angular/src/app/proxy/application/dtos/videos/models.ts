import type { IRemoteStreamContent } from '../../../volo/abp/content/models';
import type { UploadTask } from '../../../infrastructure/video-file-manager/models';

export interface StartVideoUploadDto {
  name?: string;
  description?: string;
  content: FormData;
}

export interface VideoUploadStateDto {
  id?: string;
  isCompleted: boolean;
  outputFormat?: string;
  remainingTasks: UploadTask[];
}

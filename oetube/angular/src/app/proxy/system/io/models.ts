
export interface any extends any {
  canRead: boolean;
  canWrite: boolean;
  canSeek: boolean;
  canTimeout: boolean;
  length: number;
  position: number;
  readTimeout: number;
  writeTimeout: number;
}

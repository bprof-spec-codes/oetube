import {
  CreateFFmpegOptions,
  FFmpeg,
  LogCallback,
  ProgressCallback,
  createFFmpeg,
  fetchFile,
} from '@ffmpeg/ffmpeg';
import { FFprobeWorker, FileInfo, Stream } from 'ffprobe-wasm';

import { Injectable } from '@angular/core';
import { mimeTypes } from 'mime-wrapper';

@Injectable({
  providedIn: 'root',
})
export class FFService {
  private ffmpeg: FFmpeg;
  private probe: FFprobeWorker;

  private storedFiles: Map<string, File> = new Map<string, File>();
  private cachedInfos: Map<string, FFInfo> = new Map<string, FFInfo>();

  private transcoding: boolean;
  onLogging(log: LogCallback) {
    this.ffmpeg.setLogger(log);
  }

  onProgress(progress: ProgressCallback) {
    this.ffmpeg.setProgress(progress);
  }

  async load(options?: CreateFFmpegOptions) {
    if (this.ffmpeg == undefined) {
      this.ffmpeg = createFFmpeg(options);
      this.probe = new FFprobeWorker();
    }
    if (!this.ffmpeg.isLoaded()) {
      await this.ffmpeg.load();
    }
  }

  async storeFile(file: File, fileName?: string): Promise<File> {
    fileName = fileName ? fileName : file.name;
    const type = mimeTypes.getType(fileName);
    this.deleteFile(fileName);
    file = new File([file], fileName, { type: type });
    const data = await fetchFile(file);
    this.ffmpeg.FS('writeFile', fileName, data);
    this.storedFiles.set(fileName, file);
    return file;
  }

  listFiles(): Array<string> {
    return Array.from(this.storedFiles.keys());
  }

  getFile(fileName: string): File {
    return this.storedFiles.get(fileName);
  }

  deleteFile(fileName: string): boolean {
    const exists = this.storedFiles.delete(fileName);
    if (exists) {
      this.ffmpeg.FS('unlink', fileName);
      this.cachedInfos.delete(fileName);
    }
    return exists;
  }
  hasFile(fileName: string) {
    return this.storedFiles.get(fileName);
  }

  getCommand(inputFileName: string, outputFileName, args: string): string {
    return `-i ${inputFileName} ${args} ${outputFileName}`;
  }
  async transcode(inputFileName: string, outputFileName: string, args: string): Promise<File> {
    if (!this.storedFiles.get(inputFileName)) {
      return undefined;
    }
    this.deleteFile(outputFileName);
    try {
      this.transcoding = true;
      await this.ffmpeg.run(...this.getCommand(inputFileName, outputFileName, args).split(' '));
      const data = this.ffmpeg.FS('readFile', outputFileName);
      const file = new File([data.buffer], outputFileName, {
        type: mimeTypes.getType(outputFileName),
      });
      this.storedFiles.set(outputFileName, file);
      return file;
    } finally {
      this.transcoding = false;
    }
  }
  isTranscoding(): boolean {
    return this.transcoding;
  }
  async getFileInfo(fileName: string): Promise<FFInfo> {
    if (this.storedFiles.has(fileName) && !this.cachedInfos.has(fileName)) {
      const info = new FFInfo(await this.probe.getFileInfo(this.storedFiles.get(fileName)));
      this.cachedInfos.set(fileName, info);
    }
    return this.cachedInfos.get(fileName);
  }
  async clearFiles() {
    this.listFiles().forEach(f => this.deleteFile(f));
  }
  async terminate() {
    this.clearFiles();
    this.ffmpeg.exit();
    this.probe.terminate();
  }
}
export class FFInfo {
  readonly fileName: string;
  readonly duration: number;
  readonly size: number;
  readonly bitRate: number;
  readonly formatName: string;
  readonly formatLongName: string;

  readonly audio?: FFStream;
  readonly video?: FFVideoStream;

  constructor(fileInfo: FileInfo) {
    this.fileName = fileInfo.format.filename;
    this.duration = Number.parseFloat(fileInfo.format.duration);
    this.size = Number.parseInt(fileInfo.format.size);
    this.bitRate = Number.parseInt(fileInfo.format.bit_rate);
    console.log(fileInfo.format.bit_rate);
    this.formatName = fileInfo.format.format_name;
    this.formatLongName = fileInfo.format.format_long_name;
    let stream = fileInfo.streams.find(value => {
      return value.codec_type == 'audio';
    });
    if (stream) {
      this.audio = new FFStream(stream);
    }
    stream = fileInfo.streams.find(value => {
      return (value.codec_type = 'video');
    });
    if (stream) {
      this.video = new FFVideoStream(stream);
    }
  }
}
export class FFStream {
  readonly codecName: string;
  readonly codecType: string;
  readonly codecTag: string;
  readonly duration: number;
  readonly bitRate: number;
  readonly frames: number;

  constructor(stream: Stream) {
    this.codecName = stream.codec_name;
    this.codecType = stream.codec_type;
    this.codecTag = stream.codec_tag_string;
    this.duration = Number.parseFloat(stream.duration);
    this.bitRate = Number.parseInt(stream.bit_rate);
    this.frames = Number.parseInt(stream.nb_frames);
  }
}
export class FFVideoStream extends FFStream {
  readonly width: number;
  readonly height: number;
  constructor(stream: Stream) {
    super(stream);
    this.width = stream.codec_width;
    this.height = stream.codec_height;
  }
}

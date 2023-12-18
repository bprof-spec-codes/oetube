
export interface GroupValidationDto {
  domainMaxLength: number;
  domainMinLength: number;
  domainMessage?: string;
  nameMinLength: number;
  nameMaxLength: number;
  nameMessage?: string;
  descriptionMaxLength: number;
  descriptionMessage?: string;
}

export interface PlaylistValidationDto {
  nameMinLength: number;
  nameMaxLength: number;
  nameMessage?: string;
  descriptionMaxLength: number;
  descriptionMessage?: string;
}

export interface UserValidationDto {
  nameMinLength: number;
  nameMaxLength: number;
  nameMessage?: string;
  aboutMeMaxLength: number;
  aboutMeMessage?: string;
}

export interface ValidationsDto {
  user: UserValidationDto;
  video: VideoValidationDto;
  playlist: PlaylistValidationDto;
  group: GroupValidationDto;
}

export interface VideoValidationDto {
  nameMinLength: number;
  nameMaxLength: number;
  descriptionMaxLength: number;
  nameMessage?: string;
  descriptionMessage?: string;
}

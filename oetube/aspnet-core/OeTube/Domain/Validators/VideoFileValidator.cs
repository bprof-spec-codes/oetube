using Microsoft.EntityFrameworkCore;
using OeTube.Domain.Configs;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg.Infos;
using Volo.Abp.DependencyInjection;

namespace OeTube.Domain.Validators
{
    public interface IVideoFileValidator
    {
        bool IsInDesiredResolutionAndFormat(VideoInfo videoInfo);

        void ValidateResizedVideo(Video video, VideoInfo sourceInfo, VideoInfo resizedInfo);

        void ValidateSourceVideo(VideoInfo sourceInfo);
    }

    public class VideoFileValidator : IVideoFileValidator, ITransientDependency
    {
        private IVideoFileConfig _config;

        public VideoFileValidator(IVideoFileConfig config)
        {
            _config = config;
        }

        public bool IsInDesiredResolutionAndFormat(VideoInfo videoInfo)
        {
            var resolution = videoInfo.VideoStreams[0].Resolution;
            return videoInfo.Format == _config.OutputFormat &&
                _config.GetDesiredResolutions(resolution)
                .Any(r => r == resolution);
        }

        public void ValidateSourceVideo(VideoInfo sourceInfo)
        {
            ValidateSize(sourceInfo.Size);
            ValidateSourceFormat(sourceInfo.Format);
            ValidateVideoStreams(sourceInfo.VideoStreams);
            var firstVideoStream = sourceInfo.VideoStreams[0];
            ValidateCodec(firstVideoStream.Codec);
        }

        public void ValidateResizedVideo(Video video, VideoInfo sourceInfo, VideoInfo resizedInfo)
        {
            ValidateResizedFormat(resizedInfo.Format);
            ValidateVideoStreams(resizedInfo.VideoStreams);
            var resizedVideoStream = resizedInfo.VideoStreams[0];
            ValidateResolution(video, resizedVideoStream.Resolution);
            ValidateCodec(resizedVideoStream.Codec);
            ValidateDuration(video, resizedInfo.Duration);
            ValidateResizedVideoSize(sourceInfo, resizedInfo);
            ValidateAudioStreams(sourceInfo, resizedInfo.AudioStreams);
        }

        private void ValidateSize(long size)
        {
            if (size > _config.MaxSizeInBytes)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, _config.MaxSizeInBytes.ToString());
            }
        }

        private void ValidateResizedVideoSize(VideoInfo sourceInfo, VideoInfo resizedInfo)
        {
        }

        private void ValidateSourceFormat(string format)
        {
            if (!_config.SupportedFormats.Contains(format))
            {
                throw new ArgumentException(null, nameof(format));
            }
        }

        private void ValidateResizedFormat(string format)
        {
            if (format != _config.OutputFormat)
            {
                throw new ArgumentException(null, nameof(format));
            }
        }

        private void ValidateVideoStreams(IReadOnlyCollection<VideoStreamInfo> videoStreams)
        {
            if (videoStreams.Count == 0)
            {
                throw new ArgumentException(null, nameof(videoStreams));
            }
        }

        private void ValidateCodec(string codec)
        {
            if (_config.SupportedCodecs.Count > 0 && !_config.SupportedCodecs.Contains(codec))
            {
                throw new ArgumentException(null, nameof(codec));
            }
        }

        private void ValidateDuration(Video video, TimeSpan duration)
        {
            var epsilon = 0.05;
            if (Math.Abs(video.Duration.Seconds - duration.Seconds) > epsilon)
            {
                throw new ArgumentException(null, nameof(duration));
            }
        }

        private void ValidateResolution(Video video, Resolution resolution)
        {
            if (!video.GetResolutionsBy(false).Contains(resolution))
            {
                throw new ArgumentException(null, nameof(resolution));
            }
        }

        private void ValidateAudioStreams(VideoInfo sourceInfo, IReadOnlyList<AudioStreamInfo> audioStreams)
        {
            if (sourceInfo.AudioStreams.Count != audioStreams.Count)
            {
                throw new ArgumentException(null, nameof(audioStreams));
            }
            for (int i = 0; i < audioStreams.Count; i++)
            {
                ValidateAudio(sourceInfo.AudioStreams[i], audioStreams[i]);
            }
        }

        private void ValidateAudio(AudioStreamInfo sourceAudioStream, AudioStreamInfo resizedAudioStream)
        {
            var bitRateEpsilon = 100;
            if (sourceAudioStream.Frames != resizedAudioStream.Frames &&
               sourceAudioStream.Duration != resizedAudioStream.Duration &&
               sourceAudioStream.Codec != resizedAudioStream.Codec &&
               Math.Abs(sourceAudioStream.Bitrate - resizedAudioStream.Bitrate) > bitRateEpsilon)
            {
                throw new ArgumentException(null, nameof(resizedAudioStream));
            }
        }
    }
}
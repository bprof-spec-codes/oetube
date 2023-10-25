using System.Runtime.Versioning;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using OeTube.Domain.Entities.Videos;
using OeTube.Infrastructure.FF;
using OeTube.Infrastructure.FF.Probe.Infos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace OeTube.Infrastructure.VideoFileManager
{
    public class VideoFileManager : IVideoFileManager, ITransientDependency
    {
        private const long GB = 1024L * 1024 * 1024;
        public long MaxSizeInBytes => GB;
        public string OutputFormat => "mp4";
        public Resolution DefaultResolution => Resolution.SD;
        public IEnumerable<string> GetSupportedFormats()
        {
            yield return "mp4";
        }
        public IEnumerable<string> GetSupportedCodecs()
        {
            return Enumerable.Empty<string>();
        }
        public IEnumerable<Resolution> GetPossibleResolutions()
        {
            static IEnumerable<Resolution> getResolutions()
            {
                yield return Resolution.HD;
                yield return Resolution.FHD;
            }
            return getResolutions().OrderBy(r => r.Height);
        }
        public bool IsInDesiredResolutionAndFormat(VideoInfo videoInfo)
        {
            var resolution = videoInfo.VideoStreams[0].Resolution;
            return videoInfo.Format == OutputFormat &&
                GetDesiredResolutions(resolution)
                .Any(r => r == resolution);
        }
        public IEnumerable<Resolution> GetDesiredResolutions(Resolution originalResolution)
        {
            yield return DefaultResolution;
            foreach (var item in GetPossibleResolutions())
            {
                if (originalResolution.Height < item.Height)
                {
                    break;
                }
                yield return item;
            }
        }

        public List<UploadTask> CreateUploadTasks(Video video)
        {
            return video.GetResolutionsBy(false)
                        .Select(r => new UploadTask(r))
                        .ToList();
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
            if (size > MaxSizeInBytes)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, MaxSizeInBytes.ToString());
            }
        }
        private void ValidateResizedVideoSize(VideoInfo sourceInfo, VideoInfo resizedInfo)
        {
        }
        private void ValidateSourceFormat(string format)
        {
            var supportedFormats = GetSupportedFormats();
            if (!supportedFormats.Contains(format))
            {
                throw new ArgumentException(null, nameof(format));
            }
        }
        private void ValidateResizedFormat(string format)
        {
            if (format != OutputFormat)
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
            var supportedCodecs = GetSupportedCodecs().ToList();
            if (supportedCodecs.Count > 0 && !supportedCodecs.Contains(codec))
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

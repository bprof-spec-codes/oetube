using Microsoft.Extensions.DependencyModel;
using OeTube.Infrastructure.FFmpeg.Info;
using Volo.Abp.DependencyInjection;
using XabeFFmpeg = Xabe.FFmpeg;

namespace OeTube.Infrastructure.FFmpeg
{
    [Dependency(ServiceLifetime.Transient)]
    [ExposeServices(typeof(IFFmpegService))]
    public class XabeFFmpegService : IFFmpegService
    {
        private ProbeInfo MediaInfoToProbeInfo(XabeFFmpeg.IMediaInfo media)
        {
            return new ProbeInfo()
            {
                CreationTime = media.CreationTime,
                Duration = media.Duration,
                Path = media.Path,
                Size = media.Size,
                VideoStreams = media.VideoStreams.Select(VideoStreamToVideoInfo).ToList(),
                AudioStreams = media.AudioStreams.Select(AudioStreamToAudioInfo).ToList()
            };
        }
        private VideoInfo VideoStreamToVideoInfo(XabeFFmpeg.IVideoStream video)
        {
            return new VideoInfo()
            {
                Bitrate = video.Bitrate,
                Codec = video.Codec,
                Duration = video.Duration,
                Framerate = video.Framerate,
                PixelFormat = video.PixelFormat,
                Ratio = video.Ratio,
                Width = video.Width,
                Height = video.Height,
            };
        }
        private AudioInfo AudioStreamToAudioInfo(XabeFFmpeg.IAudioStream audio)
        {
            return new AudioInfo()
            {
                Bitrate = audio.Bitrate,
                Codec = audio.Codec,
                Duration = audio.Duration,
                Channels = audio.Channels,
                Language = audio.Language,
                SampleRate = audio.SampleRate,
                Title = audio.Title,
            };
        }
        private FFmpegResult ConversionResultToFFmpegResult(FFmpegCommand command,XabeFFmpeg.IConversionResult result)
        {
            return new FFmpegResult(command, result.StartTime, result.EndTime, result.Arguments);
        }
        public async Task<ProbeInfo> AnalyzeAsync(string path, CancellationToken cancellationToken = default)
        {
            return MediaInfoToProbeInfo(await XabeFFmpeg.FFmpeg.GetMediaInfo(path, cancellationToken));
        }

        public async Task<FFmpegResult> ProcessAsync(FFmpegCommand command, Action<FFmpegCommand, int>? progress = null, CancellationToken cancellationToken = default)
        {
            var conversion = XabeFFmpeg.FFmpeg.Conversions.New();
            XabeFFmpeg.Events.ConversionProgressEventHandler? progressWrapper = null;
            if (progress != null)
            {
                progressWrapper =(sender, args) =>
                {
                    progress(command,args.Percent);
                };
                conversion.OnProgress += progressWrapper;
            }
            var result = await conversion.Start(command.Arguments, cancellationToken);
            if (progressWrapper != null)
            {
                conversion.OnProgress -= progressWrapper;
            }
            return ConversionResultToFFmpegResult(command,result);
        }
    }
}
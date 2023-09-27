using XabeFFmpeg = Xabe.FFmpeg;

namespace OeTube.External.FFmpeg.Xabe
{
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
        private FFmpegResult ConversionResultToFFmpegResult(XabeFFmpeg.IConversionResult result)
        {
            return new FFmpegResult()
            {
                Start = result.StartTime,
                End = result.EndTime,
                Arguments = result.Arguments
            };
        }
        public async Task<ProbeInfo> AnalyzeAsync(string path, CancellationToken token = default)
        {
            return MediaInfoToProbeInfo(await XabeFFmpeg.FFmpeg.GetMediaInfo(path,token));
        }

        public async Task<FFmpegResult> ProcessAsync(FFmpegArgs args, Action<int>? progress = null, CancellationToken token = default)
        {
            var conversion = XabeFFmpeg.FFmpeg.Conversions.New();
            XabeFFmpeg.Events.ConversionProgressEventHandler? progressWrapper = null;
            if (progress != null)
            {
                progressWrapper = (sender, args) =>
                {
                    progress(args.Percent);
                };
                conversion.OnProgress += progressWrapper;
            }
            var result = await conversion.Start(args.ToString(), token);
            if (progressWrapper != null)
            {
                conversion.OnProgress -= progressWrapper;
            }
            return ConversionResultToFFmpegResult(result);
        }
    }
}
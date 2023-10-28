using Newtonsoft.Json.Linq;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg.Infos;
using OeTube.Infrastructure.ProcessTemplate;
using System.Diagnostics;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.Videos.FFmpeg
{
    public class FFprobeProcess : FFProcess<VideoInfo>, ITransientDependency
    {
        public static readonly string FFprobeExe = Path.Combine(FFmpegDir, "ffprobe.exe");
        public override string PreArguments => "-v quiet -print_format json -show_format -show_streams";

        public override string ExePath => FFprobeExe;

        private VideoInfo ParseProbeOutput(string jsonOutput)
        {
            var root = JToken.Parse(jsonOutput);
            var format = root["format"] ?? throw new ArgumentException(null, nameof(jsonOutput));

            List<VideoStreamInfo> videos = new();
            List<AudioStreamInfo> audios = new();
            var streams = root["streams"];
            if (streams != null)
            {
                foreach (var item in streams)
                {
                    string? type = item["codec_type"]?.ToString();
                    if (type == "audio")
                    {
                        audios.Add(ToAudioInfo(item));
                    }
                    else if (type == "video")
                    {
                        videos.Add(ToVideoInfos(item));
                    }
                }
            }

            return new VideoInfo()
            {
                Duration = TimeSpan.FromSeconds(format.Value<double>("duration")),
                FileName = format.Value<string>("filename") ?? string.Empty,
                Size = format.Value<long>("size"),
                AudioStreams = audios,
                VideoStreams = videos
            };
        }

        private VideoStreamInfo ToVideoInfos(JToken token)
        {
            return new VideoStreamInfo()
            {
                Bitrate = token.Value<long>("bit_rate"),
                Codec = token.Value<string>("codec_name") ?? string.Empty,
                Duration = TimeSpan.FromSeconds(token.Value<double>("duration")),
                Frames = token.Value<int>("nb_frames"),
                Framerate = ToFrameRate(token["avg_frame_rate"]),
                Resolution = new Resolution(token.Value<int>("width"), token.Value<int>("height")),
                PixelFormat = token.Value<string>("pix_fmt")
            };
        }

        private double ToFrameRate(JToken? token)
        {
            double result = 0;
            if (token == null)
            {
                return result;
            }
            var frameRate = token.ToString().Split("/");
            if (frameRate.Length != 2)
            {
                return result;
            }
            if (!int.TryParse(frameRate[0], out int x))
            {
                return result;
            }
            if (!int.TryParse(frameRate[1], out int y))
            {
                return result;
            }
            if (y == 0)
            {
                return result;
            }
            return x / (double)y;
        }

        private AudioStreamInfo ToAudioInfo(JToken token)
        {
            return new AudioStreamInfo()
            {
                Bitrate = token.Value<long>("bit_rate"),
                Codec = token.Value<string>("codec_name") ?? string.Empty,
                Duration = TimeSpan.FromSeconds(token.Value<double>("duration")),
                Frames = token.Value<int>("nb_frames"),
                Channels = token.Value<int>("channels"),
                SampleRate = token.Value<int>("sample_rate")
            };
        }

        protected override VideoInfo HandleProcessOutput(Process process, ProcessSettings argument, string standardOutput, string standardError)
        {
            try
            {
                var result = ParseProbeOutput(standardOutput);
                return result;
            }
            catch
            {
                throw new ProcessException($"The output of ffprobe cannot be parsed.", null, standardOutput, standardError);
            }
        }
    }
}
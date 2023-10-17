using Newtonsoft.Json.Linq;
using OeTube.Infrastructure.FFmpeg.Info;
using OeTube.Infrastructure.ProcessTemplate;
using System.Diagnostics;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FFmpeg.Processes
{

    public class FFprobeProcess : FFProcess<ProbeInfo>, ITransientDependency
    {
        public static readonly string FFprobeExe = Path.Combine(FFmpegDir, "ffprobe.exe");
        public override string PreArguments => "-v quiet -print_format json -show_format -show_streams";

        public override string ExePath => FFprobeExe;

        private ProbeInfo ParseProbeOutput(string jsonOutput)
        {
            var root = JToken.Parse(jsonOutput);
            var format = root["format"];

            List<VideoInfo> videos = new List<VideoInfo>();
            List<AudioInfo> audios = new List<AudioInfo>();
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


            return new ProbeInfo()
            {
                Duration = TimeSpan.FromSeconds(format.Value<double>("duration")),
                FileName = format.Value<string>("filename"),
                Size = format.Value<long>("size"),
                AudioStreams = audios,
                VideoStreams = videos
            };

        }
        private VideoInfo ToVideoInfos(JToken token)
        {
            return new VideoInfo()
            {
                Bitrate = token.Value<long>("bit_rate"),
                Codec = token.Value<string>("codec_name"),
                Duration = TimeSpan.FromSeconds(token.Value<double>("duration")),
                Frames = token.Value<int>("nb_frames"),
                Framerate = ToFrameRate(token["avg_frame_rate"]),
                Width = token.Value<int>("width"),
                Height = token.Value<int>("height"),
                PixelFormat = token.Value<string>("pix_fmt"),
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
        private AudioInfo ToAudioInfo(JToken token)
        {
            return new AudioInfo()
            {
                Bitrate = token.Value<long>("bit_rate"),
                Codec = token.Value<string>("codec_name"),
                Duration = TimeSpan.FromSeconds(token.Value<double>("duration")),
                Frames = token.Value<int>("nb_frames"),
                Channels = token.Value<int>("channels"),
                SampleRate = token.Value<int>("sample_rate")
            };
        }

        protected override ProbeInfo HandleProcessOutput(Process process, ProcessSettings argument, string standardOutput, string standardError)
        {
            if (process.ExitCode != 0)
            {
                throw new FFmpegException($"FFprobe.exe exited with error code: {process.ExitCode}.", standardError);
            }
            try
            {
                var result = ParseProbeOutput(standardOutput);
                return result;
            }
            catch
            {
                throw new FFmpegException($"The output of ffprobe cannot be parsed.", standardOutput);
            }
        }
    }
}

using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FFmpeg.Infos;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FFmpeg
{
    public class ProcessUploadTaskFactory : IProcessUploadTaskFactory, ITransientDependency
    {
        public ProcessUploadTask Create(Video video, VideoInfo videoInfo)
        {
            var resolutions = video.GetResolutionsBy(true).ToArray();
            var extractFrameTarget = resolutions.OrderByDescending(r => r.Height).First();
            string hlsArguments = GetHlsArguments();

            int frameCount = 10;
            int sourceFrameCount = videoInfo.VideoStreams[0].Frames;
            string extractFramesArguments = GetExractFramesArguments(sourceFrameCount, frameCount);

            return new ProcessUploadTask(video.Id,
                                         resolutions,
                                         extractFrameTarget,
                                         video.IsAllResolutionReady(),
                                         hlsArguments,
                                         extractFramesArguments);
        }

        private string GetHlsArguments()
        {
            return $"-f hls -hls_time 6 -hls_playlist_type event -hls_segment_filename";
        }

        private string GetExractFramesArguments(int sourceFrameCount, int frameCount)
        {
            string select = string.Join("+", CreateSelectFrames(sourceFrameCount, frameCount));
            return $"-vf select='{select}' -s 480x272 -vsync 0";
        }

        private IEnumerable<string> CreateSelectFrames(int sourceFrameCount, int frameCount)
        {
            var div = (int)Math.Floor(sourceFrameCount / (double)frameCount);
            int frame = 0;
            for (int i = 0; i < frameCount; i++)
            {
                yield return $"eq(n\\,{frame})";
                frame += div;
            }
        }
    }
}
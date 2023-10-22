using OeTube.Domain.Entities.Videos;
using System.Collections.ObjectModel;

namespace OeTube.Infrastructure.FF
{
    public class ProcessUploadTask
    {
        public Guid Id { get; private set; }
        public Resolution[] Resolutions { get; private set; }
        public Resolution ExtractFrameTarget { get; private set; }
        public bool IsUploadReady { get; private set; }
        public int Frames { get; private set; }

        public ProcessUploadTask(Guid id, Resolution[] resolutions, Resolution extractFrameTarget, bool isUploadReady, int frames)
        {
            Id = id;
            Resolutions = resolutions;
            ExtractFrameTarget = extractFrameTarget;
            IsUploadReady = isUploadReady;
            Frames = frames;
        }

        public string GetHlsArguments(string listName, string segmentName)
        {
            return $"-f hls -hls_time 6 -hls_playlist_type event -hls_segment_filename {segmentName} {listName}";
        }
        public string GetExractFramesArguments(string frameName, int frameNumber)
        {
            string select = string.Join("+", CreateSelectFrames(frameNumber));
            return $"-vf select='{select}' -s 480x272 -vsync 0 {frameName}";
        }
        private IEnumerable<string> CreateSelectFrames(int frameNumber)
        {
            var div = (int)Math.Floor(Frames / (double)frameNumber);
            int frame = 0;
            for (int i = 0; i < frameNumber; i++)
            {
                yield return $"eq(n\\,{frame})";
                frame += div;
            }
        }
    }
}

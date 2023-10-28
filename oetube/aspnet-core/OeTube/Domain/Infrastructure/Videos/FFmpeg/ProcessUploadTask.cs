using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.Infrastructure.FFmpeg
{
    public class ProcessUploadTask
    {
        public Guid Id { get; private set; }
        public Resolution[] Resolutions { get; private set; }
        public Resolution ExtractFrameTarget { get; private set; }
        public bool IsUploadReady { get; private set; }
        public string HlsArguments { get; private set; }
        public string ExtractFramesArguments { get; private set; }

        public ProcessUploadTask(Guid id,
                                 Resolution[] resolutions,
                                 Resolution extractFrameTarget,
                                 bool isUploadReady,
                                 string hlsArguments,
                                 string extractFramesArguments)
        {
            Id = id;
            Resolutions = resolutions;
            ExtractFrameTarget = extractFrameTarget;
            IsUploadReady = isUploadReady;
            HlsArguments = hlsArguments;
            ExtractFramesArguments = extractFramesArguments;
        }

        public string GetHlsArguments(string listName, string segmentName)
        {
            return $"{HlsArguments} {segmentName} {listName}";
        }

        public string GetExractFramesArguments(string frameName)
        {
            return $"{ExtractFramesArguments} {frameName}";
        }
    }
}
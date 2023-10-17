using OeTube.Infrastructure.ProcessTemplate;

namespace OeTube.Infrastructure.FFmpeg.Job
{
    public class FFmpegJobArgs
    {
        public Guid JobId { get; private set; }
        public ProcessSettings[] Settings { get; private set; }
        public FFmpegJobArgs(Guid jobId, ProcessSettings[] settings)
        {
            JobId = jobId;
            Settings = settings;
        }
    }
}

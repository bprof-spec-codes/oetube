namespace OeTube.Infrastructure.FFmpeg.Job
{
    public class FFmpegJobArgs
    {
        public Guid JobId { get; }
        public FFmpegCommand[] FFmpegCommands { get; }
        public FFmpegJobArgs(Guid jobId, FFmpegCommand[] fFmpegCommands)
        {
            JobId = jobId;
            FFmpegCommands = fFmpegCommands.ToArray();
        }
    }
}

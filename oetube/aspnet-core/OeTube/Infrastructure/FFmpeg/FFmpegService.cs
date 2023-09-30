using OeTube.Infrastructure.FFmpeg.Info;
using OeTube.Infrastructure.FFmpeg.Job;
using OeTube.Infrastructure.FFmpeg.Processes;
using OeTube.Infrastructure.ProcessTemplate;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FFmpeg
{
    [Dependency(ServiceLifetime.Transient)]
    [ExposeServices(typeof(IFFmpegService))]
    public class FFmpegService:IFFmpegService
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly FFprobeProcess _ffprobe;
        private readonly FFmpegProcess _ffmpeg;
        public string WorkingDirectory
        {
            get; set;
        }

        public bool WriteToDebug
        {
            get; set;
        }

        public FFmpegService(IBackgroundJobManager jobManager,FFmpegProcess ffmpeg,FFprobeProcess ffprobe)
        {
            _ffmpeg = ffmpeg;
            _ffprobe = ffprobe;
            WorkingDirectory = "";
            WriteToDebug = false;
            _backgroundJobManager = jobManager;
        }
   
        public async Task<ProbeInfo> AnalyzeAsync(string path,CancellationToken cancellationToken = default)
        {
            return await _ffprobe.StartProcessAsync(new ProcessSettings(new NamedArguments(path), WorkingDirectory, false),cancellationToken);
        }
        public async Task<ProcessResult> ConvertAsync(NamedArguments args, CancellationToken cancellationToken = default)
        {
            return await _ffmpeg.StartProcessAsync(new ProcessSettings(args, WorkingDirectory, WriteToDebug),cancellationToken);
        }
        public async Task BulkBackgroundConvertAsync(Guid jobId,NamedArguments[] args)
        {
            FFmpegJobArgs jobArgs = new FFmpegJobArgs(jobId, args.Select(a => new ProcessSettings(a,WorkingDirectory,WriteToDebug)).ToArray());
            await _backgroundJobManager.EnqueueAsync(jobArgs);
        }
    }
}

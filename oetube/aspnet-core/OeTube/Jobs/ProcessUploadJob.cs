using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Managers;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace OeTube.Jobs
{
    public class ProcessUploadJob : AsyncBackgroundJob<ProcessUploadTask>, ITransientDependency
    {
        private readonly VideoManager _videoManager;

        public ProcessUploadJob(VideoManager videoManager)
        {
            _videoManager = videoManager;
        }

        public override async Task ExecuteAsync(ProcessUploadTask args)
        {
            await _videoManager.ProcessUploadAsync(args);
        }
    }
}
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace OeTube.Jobs
{
    public class ProcessUploadJob : AsyncBackgroundJob<ProcessVideoUploadArgs>, ITransientDependency
    {
        private readonly IProcessVideoUploadHandler _processVideoUploadHandler;

        public ProcessUploadJob(IProcessVideoUploadHandler processVideoUploadHandler)
        {
            _processVideoUploadHandler = processVideoUploadHandler;
        }

        public override async Task ExecuteAsync(ProcessVideoUploadArgs args)
        {
            await _processVideoUploadHandler.HandleFileAsync<Video>(args);
        }
    }
}
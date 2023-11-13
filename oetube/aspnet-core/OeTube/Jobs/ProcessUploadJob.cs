using OeTube.Domain.Entities.Videos;
using OeTube.Domain.FilePaths.VideoFiles;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.FileContainers;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using OeTube.Domain.Infrastructure;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Local;

namespace OeTube.Jobs
{
    public class ProcessUploadJob : AsyncBackgroundJob<ProcessVideoUploadArgs>, ITransientDependency
    {
        IProcessVideoUploadHandler _processVideoUploadHandler;
        IFileContainer _fileContainer;
        public ProcessUploadJob(IProcessVideoUploadHandler processVideoUploadHandler, IFileContainerFactory fileContainerFactory)
        {
            _processVideoUploadHandler = processVideoUploadHandler;
            _fileContainer = fileContainerFactory.Create<Video>();
        }

        public override async Task ExecuteAsync(ProcessVideoUploadArgs args)
        {
            await _processVideoUploadHandler.HandleFileAsync<Video>(args);
        }
    }
}
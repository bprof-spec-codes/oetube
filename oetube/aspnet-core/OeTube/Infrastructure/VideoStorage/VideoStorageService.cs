using Microsoft.AspNetCore.Mvc;
using OeTube.Infrastructure.FFmpeg;
using OeTube.Infrastructure.FFmpeg.Info;
using OeTube.Infrastructure.FFmpeg.Job;
using System.IO;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Http;
using Xabe.FFmpeg;

namespace OeTube.Infrastructure.VideoStorage
{
    [BlobContainerName("videos")]
    public class VideoContainer
    { }



    [Dependency(ServiceLifetime.Transient)]
    [ExposeServices(typeof(IVideoStorageService))]
    public class VideoStorageService : IVideoStorageService
    {
        private record ContainerPath(Guid Id, string Path, string AbsolutePath);

        private readonly IBlobContainer<VideoContainer> _container;
        private readonly IBlobFilePathCalculator _pathCalculator;
        private readonly IBlobContainerConfigurationProvider _containerConfigurationProvider;
        private readonly IFFmpegService _ffmpeg;
        private readonly IBackgroundJobManager _jobManager;
        public VideoStorageService(IFFmpegService ffmpeg,
                            IBlobContainer<VideoContainer> container,
                            IBlobFilePathCalculator calculator,
                            IBlobContainerConfigurationProvider provider,
                            IBackgroundJobManager backgroundJobManager
            )
        {
            _ffmpeg = ffmpeg;
            _container = container;
            _pathCalculator = calculator;
            _containerConfigurationProvider = provider;
            _jobManager = backgroundJobManager;
        }

        private ContainerPath CalculatePath(Guid id, params string[] route)
        {
            string container = BlobContainerNameAttribute.GetContainerName<VideoContainer>();
            string path = Path.Combine(id.ToString(), Path.Combine(route));
            string absolutePath = _pathCalculator.Calculate(new BlobProviderGetArgs(container, _containerConfigurationProvider.Get(container), path));
            return new ContainerPath(id, path, absolutePath);
        }

        private async Task<ProbeInfo> AnalyzeAsync(ContainerPath source, CancellationToken cancellationToken = default)
        {
            return await _ffmpeg.AnalyzeAsync(source.AbsolutePath, cancellationToken);
        }

        private void ValidateVideoFormat(ProbeInfo probeInfo)
        {
        }

        //SD 480p 720x480
        //HD 720p 1280x720
        //FHD 1080p 1920x1080
        private FFmpegJobArgs CreateJobArguments(Guid id,ContainerPath source, ProbeInfo probeInfo,CancellationToken cancellationToken)
        {
           
            return new FFmpegJobArgs(id, new FFmpegCommand[]
            {
                CreateRescaleCommand(id,source,720,480),
                CreateRescaleCommand(id,source,1280,720),
                CreateRescaleCommand(id,source,1920,1080),

            });
        }
        private FFmpegCommand CreateRescaleCommand(Guid id,ContainerPath source,int width, int height)
        {
            var output = CalculatePath(id, height + ".mp4");
            return new FFmpegCommand($"-i {source.AbsolutePath} -s {width}x{height} -c:a copy {output.AbsolutePath}");
        }
        public async Task SaveVideoAsync(Guid id, IRemoteStreamContent content, bool overrideExisting = true, CancellationToken cancellationToken = default)
        {
            using var contentStream = content.GetStream();
            var source = CalculatePath(id, "source" + Path.GetExtension(content.FileName));
            await _container.SaveAsync(source.Path, contentStream, overrideExisting, cancellationToken);
            var probeInfo = await AnalyzeAsync(source, cancellationToken);
            ValidateVideoFormat(probeInfo);
            var arguments = CreateJobArguments(id,source,probeInfo,cancellationToken);
            await Console.Out.WriteLineAsync(arguments.ToString());
            await _jobManager.EnqueueAsync(arguments);
        }

        public async Task<FileResult> GetVideoAsync(Guid id, int resolution, CancellationToken cancellationToken = default)
        {
            string outputPath = Path.Combine(id.ToString(), "output.mp4");
            var content = await _container.GetAllBytesAsync(outputPath, cancellationToken);
            return new FileContentResult(content, MimeTypes.GetByExtension("mp4"));
        }
        public Task<ProbeInfo> GetVideoInfoAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<int>> GetResolutionsAsnyc(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<Stream> GetIndexImageAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<int> GetIndexImageCountAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task SetIndexImageAsync(Guid id, int index, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetSupportedFormats()
        {
            throw new NotImplementedException();
        }
    }
}

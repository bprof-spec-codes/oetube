using Microsoft.AspNetCore.Mvc;
using OeTube.External.FFmpeg;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.Http;
using Volo.Abp.Threading;
using Xabe.FFmpeg;

namespace OeTube.Domain.Storages
{
    [BlobContainerName("videos")]
    public class VideoContainer
    { }

    public class TranscodingJob: AsyncBackgroundJob<FFmpegArgs>, ITransientDependency
    {
        private readonly IFFmpegService _ffmpeg;
        private readonly VideoStorageService _videoStorage;
        private readonly ICancellationTokenProvider _tokenProvider;
        public Guid JobId { get; }
        

        public TranscodingJob(IFFmpegService ffmpeg, VideoStorageService videoStorage, ICancellationTokenProvider tokenProvider)
        {
            _ffmpeg = ffmpeg;
            _videoStorage = videoStorage;
            _tokenProvider = tokenProvider;
            JobId = new Guid();
        }

        public async override Task ExecuteAsync(FFmpegArgs args)
        {
            await _ffmpeg.ProcessAsync(args);
        }
    }
    public record ContainerPath
    {
        public Guid Id { get; init; }
        public string Path { get; init; }
        public string AbsolutePath { get; init; }
    }
    public class VideoStorageService:DomainService
    {
        private readonly IBlobContainer<VideoContainer> _container;
        private readonly IBlobFilePathCalculator _calculator;
        private readonly IBlobContainerConfigurationProvider _provider;
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
            _calculator = calculator;
            _provider = provider;
            _jobManager = backgroundJobManager;
        }

        private ContainerPath CalculatePathFromRoot(Guid id, params string[] route)
        {
            string container = BlobContainerNameAttribute.GetContainerName<VideoContainer>();
            string path = Path.Combine(id.ToString(), Path.Combine(route));
            string videoRoot = _calculator.Calculate(new BlobProviderGetArgs(container, _provider.Get(container), path));
            return new ContainerPath()
            {
                Id = id,
                Path=path,
                AbsolutePath=videoRoot
            };
        }

        private void ValidateVideoFormat(ProbeInfo probeInfo)
        {
        }
        private async Task<ProbeInfo> AnalyzeAsync(string path, CancellationToken cancellationToken = default)
        {
            return await _ffmpeg.AnalyzeAsync(path,cancellationToken);
        }
        public async Task SaveVideoAsync(Guid id, IRemoteStreamContent content, bool overrideExisting = true, CancellationToken cancellationToken = default)
        {
            using var contentStream = content.GetStream();
            var sourcePath = CalculatePathFromRoot(id, "source" + Path.GetExtension(content.FileName));
            await _container.SaveAsync(sourcePath.Path, contentStream, overrideExisting, cancellationToken);
            var probeInfo = await AnalyzeAsync(sourcePath.AbsolutePath, cancellationToken);
            ValidateVideoFormat(probeInfo);
            var outputPath = CalculatePathFromRoot(id, "output.mp4");
            await _jobManager.EnqueueAsync(new FFmpegArgs(sourcePath.AbsolutePath,outputPath.AbsolutePath));
        }

        public async Task<FileResult> GetVideoAsync(Guid id, int resolution, CancellationToken cancellationToken=default)
        {
            string outputPath = Path.Combine(id.ToString(), "output.mp4");
            var content = await _container.GetAllBytesAsync(outputPath, cancellationToken);
            return new FileContentResult(content, MimeTypes.GetByExtension("mp4"));
        }
        public Task<ProbeInfo> GetVideoInfoAsync(Guid id, CancellationToken token=default)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<int>> GetResolutionsAsnyc(Guid id,CancellationToken token=default)
        {
            throw new NotImplementedException();
        }
        public Task<Stream> GetIndexImageAsync(Guid id, CancellationToken token=default)
        {
            throw new NotImplementedException();
        }
        public Task<int> GetIndexImageCountAsync(CancellationToken token=default)
        {
            throw new NotImplementedException();
        }
        public Task SetIndexImageAsync(Guid id, int index, CancellationToken token=default)
        {
            throw new NotImplementedException();
        }
        public  Task<bool> ExistsAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
        public  Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}

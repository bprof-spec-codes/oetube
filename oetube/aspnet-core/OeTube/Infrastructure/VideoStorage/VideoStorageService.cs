using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using OeTube.Infrastructure.FFmpeg;
using OeTube.Infrastructure.FFmpeg.Info;
using OeTube.Infrastructure.FFmpeg.Job;
using OeTube.Infrastructure.ProcessTemplate;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Http;

namespace OeTube.Infrastructure.VideoStorage
{
    [BlobContainerName("videos")]
    public class VideoContainer
    { }

    [Dependency(ServiceLifetime.Transient)]
    [ExposeServices(typeof(IVideoStorageService))]
    public class VideoStorageService : IVideoStorageService
    {
        private record Size(int Width, int Height);
        private static readonly Size SD = new Size(720, 480);
        private static readonly Size HD = new Size(1280, 720);
        private static readonly Size FHD = new Size(1920, 1080);
        private static readonly Size[] NamedResolutions = new Size[]
        {
            SD,HD,FHD
        };


        private readonly IBlobContainer<VideoContainer> _container;
        private readonly IFFmpegService _ffmpeg;
        public VideoStorageService(IFFmpegService ffmpeg,
                            IBlobContainer<VideoContainer> container,
                            IBlobFilePathCalculator calculator,
                            IBlobContainerConfigurationProvider provider
            )
        {
            _container = container;
            _ffmpeg = ffmpeg;
            _ffmpeg.WorkingDirectory = GetContainerDirectory(calculator,provider);
            _ffmpeg.WriteToDebug = true;
            
        }
        private string GetContainerDirectory(IBlobFilePathCalculator calculator,IBlobContainerConfigurationProvider provider)
        {
            string container = BlobContainerNameAttribute.GetContainerName<VideoContainer>();
            string absolutePath = Path.GetDirectoryName(calculator.Calculate(new BlobProviderGetArgs(container, provider.Get(container), "_")));
            return absolutePath;
        }
        private string GetPath(Guid id, params string[] route)
        {
            return Path.Combine(id.ToString(), Path.Combine(route));
        }
        private async Task<ProbeInfo> AnalyzeAsync(string path, CancellationToken cancellationToken)
        {
            return await _ffmpeg.AnalyzeAsync(path,cancellationToken);
        }

        private void ValidateVideoFormat(Guid id,ProbeInfo probeInfo)
        {
        }
      
        private NamedArguments[] CreateBulkArguments(Guid id,string sourcePath,IList<Size> resolutions)
        {
            var directory=Path.GetDirectoryName(sourcePath);
            NamedArguments[] args = new NamedArguments[resolutions.Count*2];
            for (int i = 0; i < resolutions.Count; i++)
            {
                int width = resolutions[i].Width;
                int height = resolutions[i].Height;
                var path = Path.Combine(directory, $"{height}.mp4");
                var hlsPath = Path.Combine(directory,height.ToString(),$"list.m3u8");
                var segmentPath = Path.Combine(directory,height.ToString(), "%d.ts");
                args[i*2]=new NamedArguments($"-i {sourcePath} -s {width}x{height} -c:a copy {path}",$"{height}.mp4");
                args[i*2+1]=new NamedArguments($"-i {path} -f hls -hls_time 6 -hls_playlist_type event -hls_segment_filename {segmentPath} {hlsPath}",$"${height}.m3u8");
            }
            return args;
        }

        private IList<Size> CreateResolutions(ProbeInfo probeInfo)
        {
            var video = probeInfo.VideoStreams.First();
            var resolutions = new List<Size>();
            int i = 0;

            while (i <NamedResolutions.Length && video.Height >= NamedResolutions[i].Height)
            {
                resolutions.Add(NamedResolutions[i]);
                i++;
            }
            return resolutions;
        }
        private async Task CreateResolutionsSegmentsFolderIfNotExistsAsync(Guid id, IList<Size> resolutions)
        {
            foreach (var item in resolutions)
            {
                string path = Path.Combine(id.ToString(), item.Height.ToString(), "create.folder");
                if(!await _container.ExistsAsync(path))
                {
                    await _container.SaveAsync(path, new byte[] { }, true);
                }
            }
        }

        public async Task SaveVideoAsync(Guid id, IRemoteStreamContent content, bool overrideExisting = true, CancellationToken cancellationToken = default)
        {
            using var contentStream = content.GetStream();
            var sourcePath = GetPath(id, "source" + Path.GetExtension(content.FileName));
            await _container.SaveAsync(sourcePath, contentStream, overrideExisting, cancellationToken);
            var probeInfo = await AnalyzeAsync(sourcePath, cancellationToken);
            ValidateVideoFormat(id,probeInfo);
            var resolutions=CreateResolutions(probeInfo);
            await CreateResolutionsSegmentsFolderIfNotExistsAsync(id, resolutions);
            var args = CreateBulkArguments(id,sourcePath, resolutions);
            await _ffmpeg.BulkBackgroundConvertAsync(id, args);
        }
        public async Task<FileStreamResult> GetM3U8Async(Guid id, int height,CancellationToken cancellationToken = default)
        {
            return new FileStreamResult(await _container.GetAsync(Path.Combine(id.ToString(),height.ToString(), $"list.m3u8")), "application/x-mpegURL");
        }
        public async Task<FileStreamResult> GetM3U8SegmentAsync(Guid id, int height,int segment, CancellationToken cancellationToken=default)
        {
            return new FileStreamResult(await _container.GetAsync(Path.Combine(id.ToString(), height.ToString(), $"{segment}.ts")), "application/x-mpegURL");
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

        public Task<FileResult> GetVideoAsync(Guid id, int resolution, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

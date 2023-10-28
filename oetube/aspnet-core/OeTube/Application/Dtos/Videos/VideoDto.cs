using OeTube.Application.Services.Url;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.Videos
{
    public class VideoMapper : IObjectMapper<Video, VideoDto>, ITransientDependency
    {
        private readonly IVideoUrlService _videoUrlService;

        public VideoMapper(IVideoUrlService videoUrlService)
        {
            _videoUrlService = videoUrlService;
        }

        public VideoDto Map(Video source)
        {
            return Map(source, new VideoDto());
        }

        public VideoDto Map(Video video, VideoDto destination)
        {
            destination.Id = video.Id;
            destination.AccessGroups = video.AccessGroups.Select(ag => ag.GroupId).ToList();
            destination.CreationTime = video.CreationTime;
            destination.CreatorId = video.CreatorId;
            destination.Description = video.Description;
            destination.Duration = video.Duration;
            destination.IndexImageSource = _videoUrlService.GetIndexImageUrl(video.Id);
            destination.IsUploadCompleted = video.IsUploadCompleted;
            destination.Name = video.Name;
            destination.PlaylistId = null;
            destination.HlsSources = video.GetResolutionsBy(true).Select(r => new HlsSourceDto()
            {
                Resolution = r,
                Src = _videoUrlService.GetHlsListUrl(video.Id, r.Width, r.Height)
            }).ToList();
            return destination;
        }
    }

    public class VideoDto
    {
        public Guid Id { get; set; }
        public List<HlsSourceDto> HlsSources { get; set; } = new();
        public string IndexImageSource { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
        public TimeSpan Duration { get; set; }
        public List<Guid> AccessGroups { get; set; } = new List<Guid>();
        public Guid? PlaylistId { get; set; }
        public bool IsUploadCompleted { get; set; }
    }
}
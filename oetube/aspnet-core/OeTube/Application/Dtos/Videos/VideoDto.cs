using OeTube.Application.Dtos.OeTubeUsers;
using OeTube.Application.Services.Url;
using OeTube.Domain.Entities.Videos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace OeTube.Application.Dtos.Videos
{
    public class VideoMapper : IObjectMapper<Video, VideoDto>, ITransientDependency
    {
        private readonly IVideoUrlService _videoUrlService;
        private readonly IObjectMapper<Guid?, CreatorDto?> _creatorMapper;
        public VideoMapper(IVideoUrlService videoUrlService, IObjectMapper<Guid?, CreatorDto?> creatorMapper)
        {
            _videoUrlService = videoUrlService;
            _creatorMapper = creatorMapper;
        }

        public VideoDto Map(Video source)
        {
            return Map(source, new VideoDto());
        }

        public VideoDto Map(Video source, VideoDto destination)
        {
            destination.Id = source.Id;
            destination.AccessGroups = source.AccessGroups.Select(ag => ag.GroupId).ToList();
            destination.CreationTime = source.CreationTime;
            destination.Description = source.Description;
            destination.Duration = source.Duration;
            destination.IndexImage = _videoUrlService.GetIndexImageUrl(source.Id);
            destination.IsUploadCompleted = source.IsUploadCompleted;
            destination.Name = source.Name;
            destination.PlaylistId = null;
            destination.HlsResolutions = source.GetResolutionsBy(true).Select(r => new HlsResolutionDto()
            {
                Width=r.Width,
                Height=r.Height,
                HlsList = _videoUrlService.GetHlsListUrl(source.Id, r.Width, r.Height)
            }).ToList();
            destination.Creator = _creatorMapper.Map(source.CreatorId);
            return destination;
        }
    }
 
    public class VideoDto:EntityDto<Guid>,IMayHaveCreatorDto
    {
        public List<HlsResolutionDto> HlsResolutions { get; set; } = new();
        public string? IndexImage { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreationTime { get; set; }
        public TimeSpan Duration { get; set; }
        public List<Guid> AccessGroups { get; set; } = new List<Guid>();
        public Guid? PlaylistId { get; set; }
        public bool IsUploadCompleted { get; set; }
        public CreatorDto? Creator { get; set; }
    }
}
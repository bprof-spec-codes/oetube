using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Repositories;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Dtos.Videos
{
    public class UpdateVideoMapper : AsyncObjectMapper<UpdateVideoDto, Video>, ITransientDependency
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ISelectVideoFrameHandler _frameHandler;

        public UpdateVideoMapper(IVideoRepository videoRepository, ISelectVideoFrameHandler frameHandler, IGroupRepository groupRepository)
        {
            _videoRepository = videoRepository;
            _frameHandler = frameHandler;
            _groupRepository = groupRepository;
        }

        public override Task<Video> MapAsync(UpdateVideoDto source)
        {
            throw new NotSupportedException();
        }

        public override async Task<Video> MapAsync(UpdateVideoDto source, Video destination)
        {
            destination.SetName(source.Name)
                       .SetDescription(source.Description)
                       .SetAccess(source.Access);

            var groups = await _groupRepository.GetManyAsync(source.AccessGroups);
            await _videoRepository.UpdateChildrenAsync(destination, groups);

            if (source.IndexImage is not null)
            {
                await _frameHandler.HandleFileAsync<Video>
                    (new SelectVideoFrameHandlerArgs(destination.Id, source.IndexImage.Value));
            }

            return destination;
        }
    }

    public class UpdateVideoDto
    {
        [StringLength(VideoConstants.NameMaxLength, MinimumLength = VideoConstants.NameMinLength)]
        public string Name { get; set; } = string.Empty;

        [StringLength(VideoConstants.NameMaxLength)]
        public string? Description { get; set; }

        public AccessType Access { get; set; } = AccessType.Public;
        public List<Guid> AccessGroups { get; set; } = new List<Guid>();

        [Range(1, int.MaxValue)]
        public int? IndexImage { get; set; }
    }
}
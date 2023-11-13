using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Repositories;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace OeTube.Application.Dtos.Videos
{
    public class StartVideoUploadMapper : AsyncObjectMapper<StartVideoUploadDto, Video>,ITransientDependency
    {
        private readonly ICurrentUser _currentUser;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IVideoRepository _videoRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IStartVideoUploadHandler _videoUploadHandler;

        public StartVideoUploadMapper(ICurrentUser currentUser,
                                      IGuidGenerator guidGenerator,
                                      IVideoRepository videoRepository,
                                      IGroupRepository groupRepository,
                                      IStartVideoUploadHandler videoUploadHandler)
        {
            _currentUser = currentUser;
            _guidGenerator = guidGenerator;
            _videoRepository = videoRepository;
            _groupRepository = groupRepository;
            _videoUploadHandler = videoUploadHandler;
        }

        public override async Task<Video> MapAsync(StartVideoUploadDto source)
        {
            var args = new StartVideoUploadHandlerArgs()
            {
                Id = _guidGenerator.Create(),
                Access = source.Access,
                Content = await ByteContent.FromRemoteStreamContentAsync(source.Content),
                CreatorId = _currentUser.Id,
                Description = source.Description,
                Name = source.Name
            };
            var video = await _videoUploadHandler.HandleFileAsync<Video>(args);
            var groups =await _groupRepository.GetManyAsync(source.AccessGroups);
            await _videoRepository.UpdateChildrenAsync(video, groups);
            return video;
        }

        public override Task<Video> MapAsync(StartVideoUploadDto source, Video destination)
        {
            throw new NotImplementedException();
        }
    }
    public class StartVideoUploadDto
    {
        [StringLength(VideoConstants.NameMaxLength, MinimumLength = VideoConstants.NameMinLength)]
        public string Name { get; set; } = string.Empty;

        [StringLength(VideoConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public AccessType Access { get; set; } = AccessType.Public;

        [Required]
        public IRemoteStreamContent? Content { get; set; }
        public List<Guid> AccessGroups { get; set; } = new List<Guid>();
    }
}
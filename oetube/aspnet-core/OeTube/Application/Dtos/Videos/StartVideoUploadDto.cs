using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FileHandlers;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace OeTube.Application.Dtos.Videos
{

    public class StartVideoUploadDtoMapper : IObjectMapper<StartVideoUploadDto, StartVideoUploadHandlerArgs>,ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentUser _currentUser;

        public StartVideoUploadDtoMapper(ICurrentUser currentUser,IGuidGenerator guidGenerator)
        {
            _currentUser = currentUser;
            _guidGenerator = guidGenerator;
        }
        public StartVideoUploadHandlerArgs Map(StartVideoUploadDto source)
        {
            return Map(source, new StartVideoUploadHandlerArgs());
        }

        public StartVideoUploadHandlerArgs Map(StartVideoUploadDto source, StartVideoUploadHandlerArgs destination)
        {
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Access = source.Access;
            destination.CreatorId = _currentUser.Id;
            destination.Id = _guidGenerator.Create();
            return destination;
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
using OeTube.Domain.Entities;
using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Domain.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;
using OeTube.Domain.Entities.Groups;
using Volo.Abp.DependencyInjection;
using OeTube.Application.Caches;

namespace OeTube.Application.Dtos.OeTubeUsers
{
    public class UpdateUserMapper : AsyncObjectMapper<UpdateUserDto, OeTubeUser>,ITransientDependency
    {
        private readonly IImageUploadHandler _imageUploadHandler;
        private readonly UserCacheService _cacheService;

        public UpdateUserMapper(IImageUploadHandler imageUploadHandler,UserCacheService cacheService)
        {
            _imageUploadHandler = imageUploadHandler;
            _cacheService = cacheService;
        }

        public override Task<OeTubeUser> MapAsync(UpdateUserDto source)
        {
            throw new NotSupportedException();

        }

        public override async Task<OeTubeUser> MapAsync(UpdateUserDto source, OeTubeUser destination)
        {
            destination.SetName(source.Name)
                       .SetAboutMe(source.AboutMe);
            if (source.Image is not null)
            {
                var content = await ByteContent.FromRemoteStreamContentAsync(source.Image);
                await _imageUploadHandler.HandleFileAsync<OeTubeUser>(new ImageUploadHandlerArgs(destination.Id, content));
            }
            await _cacheService.DeleteCreatorNameAsync(destination);
            return destination;
        }
    }
    public class UpdateUserDto
    {
        [Required]
        [StringLength(OeTubeUserConstants.NameMaxLength, MinimumLength = OeTubeUserConstants.NameMinLength)]
        public string Name { get; set; } = string.Empty;
        [StringLength(OeTubeUserConstants.AboutMeMaxLength)]
        public string? AboutMe { get; set; }

        public IRemoteStreamContent? Image { get; set; }
    }
}
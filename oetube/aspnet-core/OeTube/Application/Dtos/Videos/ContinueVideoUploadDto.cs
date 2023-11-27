using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FileHandlers;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;

namespace OeTube.Application.Dtos.Videos
{
    public class ContinueVideoUploadMapper : AsyncObjectMapper<ContinueVideoUploadDto, Video>, ITransientDependency
    {
        private readonly IContinueVideoUploadHandler _continueVideoUploadHandler;

        public ContinueVideoUploadMapper(IContinueVideoUploadHandler continueVideoUploadHandler)
        {
            _continueVideoUploadHandler = continueVideoUploadHandler;
        }

        public override Task<Video> MapAsync(ContinueVideoUploadDto source)
        {
            throw new NotSupportedException();
        }

        public override async Task<Video> MapAsync(ContinueVideoUploadDto source, Video destination)
        {
            var content = await ByteContent.FromRemoteStreamContentAsync(source.Content);
            var args = new ContinueVideoUploadHandlerArgs(destination, content);
            destination = await _continueVideoUploadHandler.HandleFileAsync<Video>(args);
            return destination;
        }
    }

    public class ContinueVideoUploadDto
    {
        [Required]
        public IRemoteStreamContent? Content { get; set; }
    }
}
using OeTube.Application.Url;
using OeTube.Domain.Entities.Videos;
using OeTube.Domain.FilePaths.VideoFiles;
using OeTube.Domain.Infrastructure.FileContainers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.Videos
{
    public class VideoIndexImagesMapper : IObjectMapper<Video, VideoIndexImagesDto>,ITransientDependency
    {
        private readonly VideoUrlService _urlService;
        private readonly IFileContainer _fileContainer;

        public VideoIndexImagesMapper(VideoUrlService urlService, IFileContainerFactory fileContainerFactory)
        {
            _urlService = urlService;
            _fileContainer = fileContainerFactory.Create<Video>();
        }

        public VideoIndexImagesDto Map(Video source)
        {
            return Map(source, new VideoIndexImagesDto());
        }

        public VideoIndexImagesDto Map(Video source, VideoIndexImagesDto destination)
        {
            var selected = _urlService.GetIndexImageUrl(source.Id);
            var indexImages = new List<string>();
            int index = 1;
            do
            {
                var name = _fileContainer.FindFile(new FramePath(source.Id, index));
                if (name == null)
                {
                    break;
                }
                indexImages.Add(_urlService.GetIndexImageByIndexUrl(source.Id, index));
                index++;
            }
            while (true);

            destination.Id = source.Id;
            destination.IndexImages = indexImages;
            destination.Selected = selected;
            return destination;
            throw new NotImplementedException();
        }
    }

    public class VideoIndexImagesDto : EntityDto<Guid>
    {
        public List<string> IndexImages { get; set; } = new List<string>();
        public string Selected { get; set; } = string.Empty;
    }
}
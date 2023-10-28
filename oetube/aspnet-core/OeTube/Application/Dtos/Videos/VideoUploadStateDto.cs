using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg;
using OeTube.Domain.Infrastructure.Videos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.Videos
{
    public class VideoUploadStateMapper : IObjectMapper<Video, VideoUploadStateDto>, ITransientDependency
    {
        private readonly IUploadTaskFactory _uploadTaskFactory;
        private readonly VideoFileConfig _config;

        public VideoUploadStateMapper(IUploadTaskFactory uploadTaskFactory, IVideoFileConfigFactory configFactory)
        {
            _uploadTaskFactory = uploadTaskFactory;
            _config = configFactory.Create();
        }

        public VideoUploadStateDto Map(Video source)
        {
            return Map(source, new VideoUploadStateDto());
        }

        public VideoUploadStateDto Map(Video source, VideoUploadStateDto destination)
        {
            destination.Id = source.Id;
            destination.OutputFormat = _config.OutputFormat;
            destination.RemainingTasks = source.GetResolutionsBy(false)
                                                .Select(_uploadTaskFactory.Create)
                                                .ToList();
            return destination;
        }
    }

    public class VideoUploadStateDto
    {
        public Guid Id { get; set; }
        public bool IsCompleted => RemainingTasks.Count == 0;
        public string OutputFormat { get; set; } = string.Empty;
        public List<UploadTask> RemainingTasks { get; set; } = new List<UploadTask>();
    }
}
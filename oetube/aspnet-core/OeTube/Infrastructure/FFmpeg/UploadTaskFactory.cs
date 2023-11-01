using OeTube.Domain.Entities.Videos;
using OeTube.Domain.Infrastructure.FFmpeg;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.FFmpeg
{
    public class UploadTaskFactory : IUploadTaskFactory, ITransientDependency
    {
        public UploadTask Create(Resolution resolution)
        {
            string scale = $"-vf scale={resolution.ToString(':')}:force_original_aspect_ratio=decrease,pad={resolution.ToString(':')}:-1:-1:color=black";
            string arguments = $"-preset ultrafast {scale} -c:a copy";
            return new UploadTask(resolution, arguments);
        }
    }
}
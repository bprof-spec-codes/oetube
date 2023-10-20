using OeTube.Domain.Entities.Videos;

namespace OeTube.Infrastructure.VideoFileManager
{
    public class UploadTask
    {
        public UploadTask(Resolution resolution,string arguments)
        {
            Resolution = resolution;
            Arguments = arguments;
        }
        public Resolution Resolution { get; }
        public string Arguments { get; }

    }

}

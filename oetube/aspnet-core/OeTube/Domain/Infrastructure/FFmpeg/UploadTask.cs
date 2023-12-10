using OeTube.Domain.Entities.Videos;

namespace OeTube.Domain.Infrastructure.FFmpeg
{
    public class UploadTask
    {
        public UploadTask(Resolution resolution, string arguments)
        {
            Resolution = resolution;
            Arguments = arguments;
        }

        public Resolution Resolution { get; }
        public string Arguments { get; }
    }
}
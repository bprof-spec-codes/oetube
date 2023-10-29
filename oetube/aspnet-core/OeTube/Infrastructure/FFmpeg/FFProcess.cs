using OeTube.Infrastructure.ProcessTemplate;

namespace OeTube.Infrastructure.FFmpeg
{
    public abstract class FFProcess<TOutput> : ProcessTemplate<TOutput>
    {
        private static string GetFFmpegDir()
        {
            string? dir = Environment.GetEnvironmentVariable("PATH")?.Split(';').FirstOrDefault(p => p.ToLower().Contains("ffmpeg"));
            if (dir is null)
            {
                throw new ProcessException(
                    "The directory of FFmpeg executables is not found in the path environment variables.\r\n" +
                    "Please install it according to the following guide: https://phoenixnap.com/kb/ffmpeg-windows");
            }
            return dir;
        }

        protected static readonly string FFmpegDir = GetFFmpegDir();
    }
}
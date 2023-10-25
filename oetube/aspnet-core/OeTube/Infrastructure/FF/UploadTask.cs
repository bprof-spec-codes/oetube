using Microsoft.CodeAnalysis.CSharp.Syntax;
using OeTube.Domain.Entities.Videos;

namespace OeTube.Infrastructure.FF
{
    public class UploadTask
    {
        public UploadTask(Resolution resolution)
        {
            Resolution = resolution;

            string scale = $"-vf scale={resolution.ToString(':')}:force_original_aspect_ratio=decrease,pad={resolution.ToString(':')}:-1:-1:color=black";

            Arguments = $"-preset ultrafast {scale} -c:a copy";
        }
        public Resolution Resolution { get; }
        public string Arguments { get; }
    }

}

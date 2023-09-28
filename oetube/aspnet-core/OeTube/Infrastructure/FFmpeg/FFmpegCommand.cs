using System.Text;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Card;

namespace OeTube.Infrastructure.FFmpeg
{

    public class FFmpegCommand
    {
        public string Arguments { get; }
        public string Name { get; }
        public FFmpegCommand(string arguments, string name="anonymous")
        {
            Arguments = arguments;
            Name = name;
        }
    }
}
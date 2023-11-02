using OeTube.Domain.Entities.Videos;

namespace OeTube.Application.Dtos.Videos
{
    public class HlsResolutionDto
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string HlsList { get; set; } = string.Empty;
    }
}
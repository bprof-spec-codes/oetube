using OeTube.Domain.Entities.Videos;

namespace OeTube.Application.Dtos.Videos
{
    public class HlsSourceDto
    {
        public Resolution Resolution { get; set; } = Resolution.Zero;
        public string Src { get; set; } = string.Empty;
    }
}
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Dtos.Videos
{

    public class VideoIndexImagesDto:EntityDto<Guid>
    {
        public List<string> IndexImages { get; set; } = new List<string>();
        public string Selected = string.Empty;
    }
}
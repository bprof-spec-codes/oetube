using OeTube.Domain.Entities.Videos;

namespace OeTube.Application.Dtos.Videos
{
    public class ResolutionSrcDto
    {
        public Resolution Resolution { get; set; } = Resolution.Zero;
        public string Src { get; set; } = string.Empty;
    }
    public class VideoDto
    {
        public Guid Id { get; set; }
        public List<ResolutionSrcDto> ResolutionsSrc { get; set; } = new();
        public string IndexImageSrc { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
        public TimeSpan Duration { get; set; }
        public List<Guid> AccessGroups { get; set; } = new List<Guid>();
        public Guid? PlaylistId { get; set; }
        public bool IsUploadCompleted { get; set; }
    }

}

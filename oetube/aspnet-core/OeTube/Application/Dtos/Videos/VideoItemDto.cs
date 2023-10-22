namespace OeTube.Application.Dtos.Videos
{
    public class VideoItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IndexImageSrc { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public Guid? PlaylistId { get; set; }

    }

}

using OeTube.Domain.Entities.Videos;

namespace OeTube.Infrastructure.VideoStorages
{
    public class VideoStoragePath : IVideoStoragePath
    {
        public string SourceName { get; } = "source";
        public string ResizedName { get; } = "resized";
        public string SelectedFrameName { get; } = "selected";
        public string HlsListName { get; } = "list";
        public string HlsListFormat { get; } = "m3u8";
        public string HlsSegmentFormat { get; } = "ts";

        public string Source(Guid id, string? format = null)
              => Path.Combine(id.ToString(), $"{SourceName}.{format ?? ""}");
        public string ResolutionDirectory(Guid id, Resolution resolution)
            => Path.Combine(id.ToString(), resolution.ToString());
        public string Resized(Guid id, Resolution resolution, string? format = null)
            => Path.Combine(ResolutionDirectory(id, resolution), $"{ResizedName}.{format ?? ""}");
        public string FramesDirectory(Guid id) => Path.Combine(id.ToString(), "frames");
        public string Frame(Guid id, int index, string? format = null)
            => Path.Combine(FramesDirectory(id), $"{index}.{format ?? ""}");
        public string SelectedFrame(Guid id, string? format = null)
            => Path.Combine(FramesDirectory(id), $"{SelectedFrameName}.{format ?? ""}");
        public string HlsList(Guid id, Resolution resolution)
            => Path.Combine(ResolutionDirectory(id, resolution), $"{HlsListName}.{HlsListFormat}");
        public string HlsSegment(Guid id, Resolution resolution, int segment)
            => Path.Combine(ResolutionDirectory(id, resolution), $"{segment}.{HlsSegmentFormat}");
    }

}

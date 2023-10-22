using OeTube.Domain.Entities.Videos;

namespace OeTube.Infrastructure.VideoStorages
{
    public interface IVideoStoragePath
    {
        string SelectedFrameName { get; }
        string HlsListName { get; }
        string HlsListFormat { get; }
        string HlsSegmentFormat { get; }
        string SourceName { get; }
        string ResizedName { get; }

        string Frame(Guid id, int index, string? format = null);
        string FramesDirectory(Guid id);
        string HlsList(Guid id, Resolution resolution);
        string HlsSegment(Guid id, Resolution resolution, int segment);
        string Resized(Guid id, Resolution resolution, string? format = null);
        string ResolutionDirectory(Guid id, Resolution resolution);
        string SelectedFrame(Guid id, string? format = null);
        string Source(Guid id, string? format = null);
    }
}
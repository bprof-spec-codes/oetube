using OeTube.Domain.Infrastructure.FileHandlers;
using OeTube.Infrastructure.FileContainers;

namespace OeTube.Domain.Infrastructure.FFmpeg
{
    public class SelectVideoFrameHandlerArgs
    {
        public SelectVideoFrameHandlerArgs(Guid id, int index)
        {
            Id = id;
            Index = index;
        }

        public Guid Id { get; set; }
        public int Index { get; set; }
    }

    public interface ISelectVideoFrameHandler:IFileHandler<SelectVideoFrameHandlerArgs>
    {
    }
}
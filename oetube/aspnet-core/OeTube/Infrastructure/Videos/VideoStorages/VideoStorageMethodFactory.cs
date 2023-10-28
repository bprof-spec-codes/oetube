using OeTube.Domain.Infrastructure.VideoStorage;
using OeTube.Infrastructure.FileContainers;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.Videos.VideoStorages
{
    public class VideoStorageMethodFactory : IVideoStorageMethodFactory, ITransientDependency
    {
        public IVideoStoragePath CreatePath()
        {
            return new VideoStoragePath();
        }

        public IVideoStorageDelete CreateDelete(IFileContainer container, IVideoStoragePath path)
        {
            return new VideoStorageDelete(container, path);
        }

        public IVideoStorageRead CreateRead(IFileContainer container, IVideoStoragePath path)
        {
            return new VideoStorageRead(container, path);
        }

        public IVideoStorageSave CreateSave(IFileContainer container, IVideoStoragePath path)
        {
            return new VideoStorageSave(container, path);
        }
    }
}
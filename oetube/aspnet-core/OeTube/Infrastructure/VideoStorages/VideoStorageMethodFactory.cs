using OeTube.Infrastructure.FileContainers;
using Volo.Abp.DependencyInjection;

namespace OeTube.Infrastructure.VideoStorages
{
    public class VideoStorageMethodFactory : IVideoStorageMethodFactory,ITransientDependency
    {
        public IVideoStoragePath CreatePath()
        {
            return new VideoStoragePath();
        }

        public IVideoStorageDelete CreateDelete(IFileContainer container, IVideoStoragePath path)
        {
            return new VideoStorageDelete(container, path);
        }
        public IVideoStorageGet CreateGet(IFileContainer container, IVideoStoragePath path)
        {
            return new VideoStorageGet(container, path);
        }
        public IVideoStorageSave CreateSave(IFileContainer container, IVideoStoragePath path)
        {
            return new VideoStorageSave(container, path);
        }
    }

}

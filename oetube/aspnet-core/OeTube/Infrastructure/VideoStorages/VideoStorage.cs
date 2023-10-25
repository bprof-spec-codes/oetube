using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using OeTube.Domain.Entities.Videos;
using OeTube.Infrastructure.FileContainers;
using Volo.Abp.DependencyInjection;


namespace OeTube.Infrastructure.VideoStorages
{
    public class VideoStorage : ITransientDependency
    {

        private readonly IFileContainer _container;
        public IVideoStoragePath Path { get; }
        public IVideoStorageSave Save { get; }
        public IVideoStorageGet Get { get; }
        public IVideoStorageDelete Delete { get; }
        public VideoStorage(IFileContainerFactory containerFactory, IVideoStorageMethodFactory methodFactory)
        {
            _container = containerFactory.Create("videos");
            Path = methodFactory.CreatePath();
            Save = methodFactory.CreateSave(_container, Path);
            Get = methodFactory.CreateGet(_container, Path);
            Delete = methodFactory.CreateDelete(_container, Path);
        }
   
    }

}

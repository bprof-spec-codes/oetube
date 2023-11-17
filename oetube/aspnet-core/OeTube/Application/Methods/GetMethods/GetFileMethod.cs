using OeTube.Domain.FilePaths;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Methods.GetMethods
{
    public class GetFileMethod<TEntity, TKey, TInputFilePath> : FileBaseMethod<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
                where TInputFilePath : IFilePath
    {
        protected virtual string? FileName { get; set; }

        public GetFileMethod(IAbpLazyServiceProvider serviceProvider,
                             IReadRepository<TEntity, TKey> repository,
                             IFileContainer fileContainer) : base(serviceProvider, repository, fileContainer)
        {
        }

        public virtual GetFileMethod<TEntity, TKey, TInputFilePath> SetFileName(string? fileName)
        {
            FileName = fileName;
            return this;
        }

        protected virtual string GetFullFileName(TKey id, TInputFilePath input)
        {
            return $"{typeof(TEntity).Name.ToLower()}_{id}_{FileName}";
        }

        protected virtual async Task<ByteContent> GetFileMethodAsync(TInputFilePath input)
        {
            return await FileContainer.GetFileAsync(input);
        }

        public virtual async Task<IRemoteStreamContent> GetFileAsync(TKey id, TInputFilePath input)
        {
            await CheckPolicyAsync();
            var entity = await GetByIdAsync(id, false);
            await CheckRightsAsync(entity);
            string name = GetFullFileName(id, input);
            var content = await GetFileMethodAsync(input);
            return content.GetRemoteStreamContent(name);
        }
    }
}
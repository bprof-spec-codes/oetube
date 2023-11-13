using OeTube.Domain.FilePaths;
using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FileContainers;
using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Methods.GetMethods
{
    public class GetDefaultFileMethod<TEntity, TKey, TInputFilePath> :
            GetFileMethod<TEntity, TKey, TInputFilePath>
            where TEntity : class, IEntity<TKey>
                where TInputFilePath : IDefaultFilePath

    {
        public GetDefaultFileMethod(IAbpLazyServiceProvider serviceProvider,
                                    IReadRepository<TEntity, TKey> repository,
                                    IFileContainer fileContainer) : base(serviceProvider, repository, fileContainer)
        {
        }

        protected virtual string? DefaultFileName { get; set; }

        protected virtual string GetFullDefaultFileName()
        {
            return $"{nameof(TEntity).ToLower()}_{DefaultFileName}";
        }

        public GetDefaultFileMethod<TEntity, TKey, TInputFilePath> SetDefaultFileName(string? fileName)
        {
            DefaultFileName = fileName;
            return this;
        }

        public override GetDefaultFileMethod<TEntity, TKey, TInputFilePath> SetFileName(string? filename)
        {
            base.SetFileName(filename);
            return this;
        }

        protected override async Task<ByteContent> GetFileMethodAsync(TInputFilePath input)
        {
            return await FileContainer.GetFileOrDefaultAsync(input);
        }

        protected virtual async Task<ByteContent> GetDefaultFileMethodAsync()
        {
            return await FileContainer.GetDefaultFileAsync<TInputFilePath>();
        }

        public async Task<IRemoteStreamContent> GetDefaultFileAsync()
        {
            await CheckPolicyAsync();
            var name = GetFullDefaultFileName();
            var content = await GetDefaultFileMethodAsync();
            return content.GetRemoteStreamContent(name);
        }
    }
}
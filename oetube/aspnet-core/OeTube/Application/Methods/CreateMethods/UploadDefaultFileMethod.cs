using OeTube.Domain.Infrastructure;
using OeTube.Domain.Infrastructure.FileHandlers;
using Volo.Abp.Content;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Methods.CreateMethods
{
    public class UploadDefaultFileMethod<TEntity, TFileHandler> : ApplicationMethod
        where TEntity : class, IEntity
        where TFileHandler : IFileHandler<ByteContent>
    {
        public UploadDefaultFileMethod(IAbpLazyServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected virtual TFileHandler GetFileHandler()
        {
            return ServiceProvider.LazyGetRequiredService<TFileHandler>();
        }

        public async Task UploadFile(IRemoteStreamContent input)
        {
            await CheckPolicyAsync();
            var content = await ByteContent.FromRemoteStreamContentAsync(input);

            await GetFileHandler().HandleFileAsync<TEntity>(content);
        }
    }
}
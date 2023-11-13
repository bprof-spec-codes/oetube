using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Methods.DeleteMethods
{
    public interface IDeleteMethod<TKey>
    {
        Task DeleteAsync(TKey id);
    }

    public class DeleteMethod<TEntity, TKey> : GetBaseMethod<TEntity, TKey>, IDeleteMethod<TKey> where TEntity : class, IEntity<TKey>
    {
        protected override IDeleteRepository<TEntity, TKey> Repository { get; }

        public DeleteMethod(IAbpLazyServiceProvider serviceProvider,
                            IDeleteRepository<TEntity, TKey> repository) : base(serviceProvider, repository)
        {
            Repository = repository;
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            await CheckPolicyAsync();
            var entity = await Repository.GetAsync(id);
            await CheckRightsAsync(entity);
            await Repository.DeleteAsync(entity, true);
        }
    }
}
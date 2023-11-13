using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Methods.GetMethods
{
    public interface IGetMethod<TKey, TOutputDto>
    {
        Task<TOutputDto> GetAsync(TKey id);
    }

    public class GetMethod<TEntity, TKey, TOutputDto> :
            GetBaseMethod<TEntity, TKey>, IGetMethod<TKey, TOutputDto> where TEntity : class, IEntity<TKey>
    {
        public GetMethod(IAbpLazyServiceProvider serviceProvider, IReadRepository<TEntity, TKey> repository) : base(serviceProvider, repository)
        {
        }

        public virtual async Task<TOutputDto> GetAsync(TKey id)
        {
            await CheckPolicyAsync();
            var entity = await GetByIdAsync(id, true);
            await CheckRightsAsync(entity);
            var output = await MapAsync<TEntity, TOutputDto>(entity);
            return output;
        }
    }
}
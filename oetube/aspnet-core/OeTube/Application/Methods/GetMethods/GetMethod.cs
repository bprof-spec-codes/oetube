using OeTube.Application.AuthorizationCheckers;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Methods.GetMethods
{
   
    public class GetMethod<TEntity, TKey, TOutputDto> :
            GetBaseMethod<TEntity, TKey> where TEntity : class, IEntity<TKey>
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
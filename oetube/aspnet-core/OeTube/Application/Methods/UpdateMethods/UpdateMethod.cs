using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EventBus.Local;

namespace OeTube.Application.Methods.UpdateMethods
{
    public class UpdateMethod<TEntity, TKey, TInputDto, TOutputDto> : GetBaseMethod<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected override IUpdateRepository<TEntity, TKey> Repository { get; }

        public UpdateMethod(IAbpLazyServiceProvider serviceProvider,
                               IUpdateRepository<TEntity, TKey> repository) :
            base(serviceProvider, repository)
        {
            Repository = repository;
        }

        public virtual async Task<TOutputDto> UpdateAsync(TKey id, TInputDto input)
        {
            await CheckPolicyAsync();
            var entity = await GetByIdAsync(id, true);
            await CheckRightsAsync(entity);
            entity = await MapAsync(input, entity);
            var updatedEntity = await Repository.UpdateAsync(entity, true);
            await ServiceProvider.GetRequiredService<ILocalEventBus>()
                                 .PublishAsync(new EntityUpdatingEventData<TEntity>(updatedEntity));

            var output = await MapAsync<TEntity, TOutputDto>(updatedEntity);
            return output;
        }
    }
}
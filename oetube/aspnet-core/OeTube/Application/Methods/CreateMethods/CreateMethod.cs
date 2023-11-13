using OeTube.Domain.Repositories.CustomRepository;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace OeTube.Application.Methods.CreateMethods
{
    public interface ICreateMethod<TInputDto, TOutputDto>
    {
        Task<TOutputDto> CreateAsync(TInputDto input);
    }

    public class CreateMethod<TEntity, TKey, TInputDto, TOutputDto>
            : ApplicationMethod, ICreateMethod<TInputDto, TOutputDto> where TEntity : class, IEntity
    {
        protected virtual IInsertRepository<TEntity> Repository { get; }

        public CreateMethod(IAbpLazyServiceProvider serviceProvider,
                            IInsertRepository<TEntity> repository) : base(serviceProvider)
        {
            Repository = repository;
        }

        public virtual async Task<TOutputDto> CreateAsync(TInputDto input)
        {
            await CheckPolicyAsync();
            var entity = await MapAsync<TInputDto, TEntity>(input);
            entity = await Repository.InsertAsync(entity, true);
            var output = await MapAsync<TEntity, TOutputDto>(entity);
            return output;
        }
    }
}
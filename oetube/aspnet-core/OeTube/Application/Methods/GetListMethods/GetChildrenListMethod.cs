using OeTube.Application.AuthorizationCheckers;
using OeTube.Application.Dtos;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace OeTube.Application.Methods.GetListMethods
{
    public class GetChildrenListMethod<TEntity, TKey, TChildEntity, TChildQueryArgs, TChildOutputListItemDto>
        : GetBaseMethod<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TChildEntity : class, IEntity
        where TChildQueryArgs : IQueryArgs
    {
        protected override IChildQueryRepository<TEntity, TKey, TChildEntity, TChildQueryArgs> Repository { get; }

        public GetChildrenListMethod(IAbpLazyServiceProvider serviceProvider,
                                  IChildQueryRepository<TEntity, TKey, TChildEntity, TChildQueryArgs> repository) : base(serviceProvider, repository)
        {
            Repository = repository;

        }

 
        public virtual async Task<PaginationDto<TChildOutputListItemDto>> GetCustomChildrenListAsync<TRepository>(TKey id,Func<TRepository,TEntity,Task<PaginationResult<TChildEntity>>> getListMethod)
        where TRepository:class, IChildQueryRepository<TEntity,TKey,TChildEntity,TChildQueryArgs>
        {
            var repository = Repository as TRepository ?? throw new InvalidCastException(nameof(TRepository));
            await CheckPolicyAsync();
            var entity = await GetByIdAsync(id);
            await CheckRightsAsync(entity);

            PaginationResult<TChildEntity> pagination = await getListMethod(repository,entity);
            if (Authorization is IAuthorizationManyChecker<TChildEntity> manyChecker)
            {
                await manyChecker.CheckRightsManyAsync(pagination);
            }
            var dtos = new PaginationDto<TChildOutputListItemDto>();
            foreach (var item in pagination)
            {
                var dto = await MapAsync<TChildEntity, TChildOutputListItemDto>(item);
                dtos.Items.Add(dto);
            }
            return dtos;
        }
        public virtual async Task<PaginationDto<TChildOutputListItemDto>> GetChildrenListAsync(TKey id, TChildQueryArgs input)
        {
                if (Repository is IChildQueryAvaliableRepository<TEntity, TKey, TChildEntity, TChildQueryArgs> childQueryAvaliable)
                {
                    var requesterId = ServiceProvider.GetRequiredService<ICurrentUser>().Id;
                    return await GetCustomChildrenListAsync<IChildQueryAvaliableRepository<TEntity, TKey, TChildEntity, TChildQueryArgs>>
                        (id, (repository, entity) => repository.GetAvaliableChildrenAsync(requesterId, entity, input));
                }
                else
                {
                    return await GetCustomChildrenListAsync<IChildQueryRepository<TEntity, TKey, TChildEntity, TChildQueryArgs>>
                        (id, (repository, entity) => repository.GetChildrenAsync(entity, input));
                }
        }
    }
}
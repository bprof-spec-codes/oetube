using IdentityModel;
using OeTube.Application.AuthorizationCheckers;
using OeTube.Application.Dtos;
using OeTube.Application.Dtos.Groups;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace OeTube.Application.Methods.GetListMethods
{

    public class GetListMethod<TEntity, TQueryArgs, TOutputListItemDto> : ApplicationMethod
            where TEntity : class, IEntity
            where TQueryArgs : IQueryArgs
    {
        public GetListMethod(IAbpLazyServiceProvider serviceProvider, IQueryRepository<TEntity, TQueryArgs> repository) : base(serviceProvider)
        {
            Repository = repository;
        }

        protected virtual IQueryRepository<TEntity, TQueryArgs> Repository { get; }

        public virtual async Task<PaginationDto<TOutputListItemDto>> GetCustomListAsync<TRepository>(Func<TRepository,Task<PaginationResult<TEntity>>> getListMethod)
            where TRepository:class,IQueryRepository<TEntity,TQueryArgs>
        {
            var repository = Repository as TRepository ?? throw new InvalidCastException(nameof(TRepository));
            await CheckPolicyAsync();
            PaginationResult<TEntity> pagination = await getListMethod(repository);
            if (Authorization is IAuthorizationManyChecker<TEntity> manyChecker)
            {
                await manyChecker.CheckRightsManyAsync(pagination);
            }
            var dtos = new PaginationDto<TOutputListItemDto>()
            {
                Items = new List<TOutputListItemDto>(),
                Skip = pagination.Skip,
                Take = pagination.Take,
                TotalCount = pagination.TotalCount
            };
            foreach (var entity in pagination)
            {
                var dto = await MapAsync<TEntity, TOutputListItemDto>(entity);
                dtos.Items.Add(dto);
            }
            return dtos;
        }
        public virtual async Task<PaginationDto<TOutputListItemDto>> GetListAsync(TQueryArgs input)
        {
            var isAdmin=ServiceProvider.GetRequiredService<ICurrentUser>().IsInRole("admin");
            if (Repository is IQueryAvaliableRepository<TEntity, TQueryArgs> && !isAdmin)
            {
                var requesterId = ServiceProvider.GetRequiredService<ICurrentUser>().Id;
                return await GetCustomListAsync<IQueryAvaliableRepository<TEntity, TQueryArgs>>((repository) => repository.GetAvaliableAsync(requesterId, input));
            }
            else
            {
                return await GetCustomListAsync<IQueryRepository<TEntity, TQueryArgs>>((repository) => repository.GetListAsync(input));
            }

        }
    }
}
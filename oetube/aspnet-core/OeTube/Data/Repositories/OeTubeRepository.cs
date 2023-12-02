﻿using Microsoft.EntityFrameworkCore;
using OeTube.Domain.Repositories;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict;

namespace OeTube.Data.Repositories
{
    public abstract class OeTubeRepository<TEntity, TKey, TIncluder, TFilter, TQueryArgs> : EfCoreRepository<OeTubeDbContext, TEntity, TKey>, ICustomRepository<TEntity, TKey, TQueryArgs>
       where TEntity : class, IEntity<TKey>
       where TQueryArgs : IQueryArgs
       where TFilter : IFilter<TEntity, TQueryArgs>
       where TIncluder : IIncluder<TEntity>
    {
        protected OeTubeRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<TEntity>> WithDetailsAsync()
        {
            return Include<TEntity, TIncluder>(await GetQueryableAsync<TEntity>(), true);
        }

        public async Task<PaginationResult<TEntity>> GetListAsync(TQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await CreateListAsync<TEntity, TIncluder, TFilter, TQueryArgs>
                (await GetQueryableAsync<TEntity>(), args, includeDetails, cancellationToken);
        }

        public async Task<List<TEntity>> GetManyAsync(IEnumerable<TKey> ids, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await GetManyAsync<TEntity, TKey, TIncluder>(ids, includeDetails, cancellationToken);
        }

        protected async Task<IQueryable<T>> GetQueryableAsync<T>()
            where T : class, IEntity
        {
            return (await GetDbContextAsync()).Set<T>();
        }

        protected async Task<DbSet<TSetEntity>> GetDbSetAsync<TSetEntity>()
        where TSetEntity : class, IEntity
        {
            return (await GetDbContextAsync()).Set<TSetEntity>();
        }

        protected async Task<TCreator?> GetCreatorAsync<TCreatedEntity, TCreator, TCreatorIncluder>(TCreatedEntity entity, bool includeDetails = true, CancellationToken cancellationToken = default)
        where TCreatedEntity : class, IEntity, IMayHaveCreator
        where TCreator : class, IEntity<Guid>
        where TCreatorIncluder : IIncluder<TCreator>
        {
            if (entity.CreatorId is null) return null;
            return await GetAsync<TCreator, Guid, TCreatorIncluder>(entity.CreatorId.Value, includeDetails, cancellationToken);
        }

        protected async Task<TGetEntity> GetAsync<TGetEntity, TEntityKey, TEntityIncluder>(TEntityKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        where TGetEntity : class, IEntity<TEntityKey>
        where TEntityIncluder : IIncluder<TGetEntity>
        {
            var queryable = Include<TGetEntity, TEntityIncluder>((await GetQueryableAsync<TGetEntity>()).OrderBy(e => e.Id), includeDetails);
            var entity = await queryable.FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
            return entity ?? throw new EntityNotFoundException(typeof(TGetEntity), id);
        }

        protected IQueryable<TIncludedEntity> Include<TIncludedEntity, TEntityIncluder>(IQueryable<TIncludedEntity> queryable, bool includeDetails)
          where TIncludedEntity : class, IEntity
          where TEntityIncluder : IIncluder<TIncludedEntity>
        {
            if (includeDetails)
            {
                var includer = LazyServiceProvider.GetRequiredService<TEntityIncluder>();
                queryable = includer.Include(queryable);
            }
            return queryable;
        }

        protected async Task<PaginationResult<TListEntity>> CreateListAsync<TListEntity>(IQueryable<TListEntity> queryable,
                                                                                        IQueryArgs? args,
                                                                                        CancellationToken cancellationToken)
         where TListEntity : class, IEntity
        {
            if (!await queryable.AnyAsync(cancellationToken))
            {
                return new PaginationResult<TListEntity>();
            }
            try
            {
                queryable = queryable.OrderByIf<TListEntity, IQueryable<TListEntity>>(args?.Sorting is not null, args!.Sorting!);
            }
            catch { }

            int itemPerPage = (int)(args?.ItemPerPage ?? ItemPerPage.P10);

            int totalCount = queryable.Count();
            (int quotient, int remainder) = Math.DivRem(totalCount, itemPerPage);
            int totalPage = remainder > 0 ? quotient + 1 : quotient;

            int page = args?.Page ?? 0;
            page = page >= 0 ? page : 0;
            page = page < totalPage ? page : totalPage - 1;

            queryable = queryable.Skip(page * itemPerPage);
            queryable = queryable.Take(itemPerPage);

            var items = await queryable.ToListAsync(cancellationToken);
            return new PaginationResult<TListEntity>()
            {
                CurrentPage = page,
                PageCount = totalPage,
                Items = items,
                TotalCount = totalCount
            };
        }

        protected async Task<PaginationResult<TListEntity>> CreateListAsync<TListEntity, TEntityIncluder>(IQueryable<TListEntity> queryable,
                                                                                                               IQueryArgs? args,
                                                                                                               bool includeDetails,
                                                                                                               CancellationToken cancellationToken)
            where TListEntity : class, IEntity
            where TEntityIncluder : IIncluder<TListEntity>
        {
            queryable = Include<TListEntity, TEntityIncluder>(queryable, includeDetails);
            return await CreateListAsync(queryable, args, cancellationToken);
        }

        protected async Task<PaginationResult<TListEntity>> CreateListAsync<TListEntity, TEntityIncluder, TEntityFilter, TEntityQueryArgs>
            (IQueryable<TListEntity> queryable, TEntityQueryArgs? args, bool includeDetails = false, CancellationToken cancellationToken = default)
            where TListEntity : class, IEntity
            where TEntityFilter : IFilter<TListEntity, TEntityQueryArgs>
            where TEntityQueryArgs : IQueryArgs
            where TEntityIncluder : IIncluder<TListEntity>
        {
            if (args is not null)
            {
                var filter = LazyServiceProvider.GetRequiredService<TEntityFilter>();
                queryable = filter.FilterQueryable(queryable, args);
            }
            return await CreateListAsync<TListEntity, TEntityIncluder>(queryable, args, includeDetails, cancellationToken);
        }

        protected async Task<List<TManyEntity>> GetManyAsync<TManyEntity, TEntityKey, TEntityIncluder>
        (IEnumerable<TEntityKey> ids, bool includeDetails, CancellationToken cancellationToken)
           where TManyEntity : class, IEntity<TEntityKey>
           where TEntityIncluder : IIncluder<TManyEntity>
        {
            var queryable = (await GetQueryableAsync<TManyEntity>()).Where(e => ids.Contains(e.Id));
            if (includeDetails)
            {
                var includer = LazyServiceProvider.LazyGetRequiredService<TEntityIncluder>();
                queryable = includer.Include(queryable);
            }

            return await queryable.ToListAsync(cancellationToken);
        }
    }
}
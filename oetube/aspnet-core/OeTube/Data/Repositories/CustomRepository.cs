using Microsoft.EntityFrameworkCore;
using OeTube.Domain.Repositories.CustomRepository;
using OeTube.Domain.Repositories.QueryArgs;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.OpenIddict;

namespace OeTube.Data.Repositories
{
    public abstract class CustomRepository<TEntity, TKey, TIncluder, TFilter, TQueryArgs> :
       EfCoreRepository<OeTubeDbContext, TEntity, TKey>, ICustomRepository<TEntity, TKey, TQueryArgs>
       where TEntity : class, IEntity<TKey>
       where TIncluder : IIncluder<TEntity>
       where TQueryArgs : IQueryArgs
       where TFilter : Filter<TEntity, TQueryArgs>
    {
        protected TIncluder _includer;
        protected TFilter _filter;

        protected CustomRepository(IDbContextProvider<OeTubeDbContext> dbContextProvider, TIncluder includer, TFilter filter) : base(dbContextProvider)
        {
            _includer = includer;
            _filter = filter;
        }

        protected async Task<IQueryable<T>> GetQueryableAsync<T>()
         where T : class, IEntity
        {
            return (await GetDbContextAsync()).Set<T>();
        }

        protected async Task<List<T>> ListAsync<T, TArgs>(IQueryable<T> queryable,
                                                           IIncluder<T>? includer,
                                                           Filter<T, TArgs>? filter,
                                                           TArgs? queryArgs,
                                                           bool includeDetails,
                                                           CancellationToken cancellationToken)
         where T : class, IEntity
        where TArgs : IQueryArgs
        {
            int count = await queryable.CountAsync(cancellationToken);
            if (count <= 0)
            {
                return new List<T>();
            }

            if (queryArgs is null)
            {
                return await queryable.ToListAsync(cancellationToken);
            }

            if (filter is not null)
            {
                queryable = filter.FilterQueryable(queryable, queryArgs);
            }
            try
            {
                queryable = queryable.OrderByIf<T, IQueryable<T>>(queryArgs.Sorting is not null, queryArgs.Sorting ?? "");
            }
            catch
            { }

            if (queryArgs.SkipCount is not null && queryArgs.SkipCount > 0)
            {
                queryable = queryable.Skip(queryArgs.SkipCount.Value);
            }
            if (queryArgs.MaxResultCount is not null && queryArgs.MaxResultCount >= 0)
            {
                queryable = queryable.Take(queryArgs.MaxResultCount.Value);
            }
            if (includer is not null)
            {
                queryable = includer.Include(queryable, includeDetails);
            }

            return await queryable.ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<TEntity>> WithDetailsAsync()
        {
            var queryable = await GetQueryableAsync();
            return _includer.Include(queryable, true);
        }

        public async Task<List<TEntity>> GetManyAsync(IEnumerable<TKey> ids, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var queryable = await (includeDetails ? WithDetailsAsync() : GetQueryableAsync());
            return await queryable.Where(e => ids.Contains(e.Id)).ToListAsync(cancellationToken);
        }

        public async Task<List<TEntity>> GetListAsync(TQueryArgs? args = default, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await ListAsync(await GetQueryableAsync(), _includer, _filter, args, includeDetails, cancellationToken);
        }
    }
}
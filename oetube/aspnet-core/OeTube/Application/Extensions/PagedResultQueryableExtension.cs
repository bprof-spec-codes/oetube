using Microsoft.EntityFrameworkCore;
using OeTube.Domain.Repositories.Extensions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Extensions
{
    public static class PagedResultQueryableExtension
    {
        public async static Task<PagedResultDto<TDestination>> ToPagedResultDtoAsync<TSource, TDestination>
         (this IQueryable<TSource> queryable,
         IObjectMapper mapper, IPagedAndSortedResultRequest request, bool includeDetails = false, CancellationToken cancellationToken = default)
         where TSource : class, IEntity
        {
            var result = queryable.Include(includeDetails)
                                  .ToPagedAndSorted(request).AsAsyncEnumerable();
            var list = new List<TDestination>();
            await foreach (var item in result.WithCancellation(cancellationToken))
            {
                list.Add(mapper.Map<TSource, TDestination>(item));
            };

            return new PagedResultDto<TDestination>(list.Count, list);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using OeTube.Domain.Repositories.Extensions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectMapping;

namespace OeTube.Application.Extensions
{
    public static class PagedResultQueryableExtension
    {
        public static IQueryable<T> ToPagedAndSorted<T>(this IQueryable<T> queryable,
          IPagedAndSortedResultRequest request)
        {
            return queryable.OrderByIf<T, IQueryable<T>>(!request.Sorting.IsNullOrWhiteSpace(), request.Sorting)
                            .PageBy(request.SkipCount, request.MaxResultCount);
        }

        public async static Task<PagedResultDto<TDestination>> ToPagedResultDtoAsync<TSource, TDestination>
            (this IQueryable<TSource> queryable,
            Func<TSource,TDestination> mapper, IPagedAndSortedResultRequest request, CancellationToken cancellationToken = default)
            where TSource : class, IEntity
        {
            var result = queryable.ToPagedAndSorted(request);
            var list = new List<TDestination>();
            await foreach (var item in result.AsAsyncEnumerable().WithCancellation(cancellationToken))
            {
                list.Add(mapper(item));
            }

            return new PagedResultDto<TDestination>(list.Count, list);
        }
    }
}

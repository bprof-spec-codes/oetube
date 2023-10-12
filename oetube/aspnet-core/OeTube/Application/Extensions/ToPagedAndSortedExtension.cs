using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NUglify;
using OeTube.Entities;
using System.Linq.Expressions;
using System.Threading;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Validation.StringValues;

namespace OeTube.Domain.Repositories.Extensions
{
    public static class ToPagedAndSortedExtension
    {

        public static IQueryable<T> ToPagedAndSorted<T>(this IQueryable<T> queryable,
            IPagedAndSortedResultRequest request)
        {
            return queryable.OrderByIf<T, IQueryable<T>>(!request.Sorting.IsNullOrWhiteSpace(), request.Sorting)
                            .PageBy(request.SkipCount, request.MaxResultCount);
        }
        public async static Task<PagedResultDto<TDestination>> ToPagedResultDtoAsync<TSource, TDestination>
            (this IQueryable<TSource> queryable, 
            IObjectMapper mapper, IPagedAndSortedResultRequest request, CancellationToken cancellationToken=default)
            where TSource : class, IEntity
        {
            var result = queryable.ToPagedAndSorted(request);
            var list = new List<TDestination>();
            await foreach (var item in result.AsAsyncEnumerable().WithCancellation(cancellationToken))
            {
                list.Add(mapper.Map<TSource, TDestination>(item));
            }

            return new PagedResultDto<TDestination>(list.Count, list);
        }
    }
}

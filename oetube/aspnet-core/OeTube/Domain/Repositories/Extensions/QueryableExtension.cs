using Microsoft.CodeAnalysis.CSharp.Syntax;
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

namespace OeTube.Domain.Repositories.Extensions
{
    public static class QueryableExtension
    {

        public static IQueryable<T> ToPagedAndSorted<T>(this IQueryable<T> queryable,
            IPagedAndSortedResultRequest request)
        {
            return queryable.OrderByIf<T, IQueryable<T>>(!request.Sorting.IsNullOrWhiteSpace(), request.Sorting)
                            .PageBy(request.SkipCount, request.MaxResultCount);
        }
        public static PagedResultDto<TDestination> ToPagedResultDto<TSource, TDestination>
            (this IQueryable<TSource> queryable, 
            IObjectMapper mapper, IPagedAndSortedResultRequest request,bool includeDetails = false)
            where TSource : class, IEntity
        {
            var result = queryable.Include(includeDetails)
                                  .ToPagedAndSorted(request)
                                  .Select(mapper.Map<TSource, TDestination>)
                                  .ToList();
            return new PagedResultDto<TDestination>(result.Count, result);
        }
    }
}

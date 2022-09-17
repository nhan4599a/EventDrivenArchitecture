using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;
using SortDirection = Infrastructure.Models.SortDirection;

namespace Infrastructure.Extensions
{
    public static class RelationalDbQueryExtensions
    {
        public static IOrderedQueryable<TEntity> Sort<TEntity>(
            this IQueryable<TEntity> query,
            SortRequestCollection<TEntity> sortRequests)
        {
            foreach (var sortRequest in sortRequests)
            {
                var @params = new object[] { sortRequest.SortExpression };
                query = (IOrderedQueryable<TEntity>)
                    query.GetSuitableSortMethod(sortRequest.SortDirection).Invoke(query, @params)!;
            }
            return (IOrderedQueryable<TEntity>)query;
        }

        public static async Task<PagedList<TEntity>> ToPagedListAsync<TEntity>(
            this IQueryable<TEntity> query,
            PaginationRequest paginationRequest)
            where TEntity : class
        {
            query.ThrowIfNotOrdered();

            var noTrackingQuery = query.AsNoTracking();

            var totalItemsCountTask = noTrackingQuery.DeferredCount().FutureValue().ValueAsync();

            var pageIndex = paginationRequest.PageIndex;
            var pageSize = paginationRequest.PageSize;

            var itemsTask = noTrackingQuery
                                .Skip(paginationRequest.GetSkipNumber())
                                .Take(pageSize)
                                .Future()
                                .ToArrayAsync();

            await Task.WhenAll(totalItemsCountTask, itemsTask);

            var pagedList = new PagedList<TEntity>(pageIndex, pageSize, await itemsTask, await totalItemsCountTask);

            return pagedList;
        }

        private static MethodInfo GetSuitableSortMethod<TEntity>(this IQueryable<TEntity> query, SortDirection sortDirection)
        {
            var methodNamePrefix = query.IsOrdered() ? "Then" : "Order";
            var methodNamePostfix = sortDirection.GetPostfixSortMethodName();
            var methodName = $"{methodNamePrefix}By{methodNamePostfix}";
            return typeof(Queryable).GetMethod(methodName)!;
        }

        internal static bool IsOrdered<TEntity>(this IQueryable<TEntity> query)
        {
            return query.Expression.Type == typeof(IOrderedQueryable<TEntity>);
        }
    }
}

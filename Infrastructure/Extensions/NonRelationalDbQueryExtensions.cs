using Infrastructure.Constants;
using Infrastructure.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SortDirection = Infrastructure.Models.SortDirection;

namespace Infrastructure.Extensions
{
    public static class NonRelationalDbQueryExtensions
    {
        public static IOrderedFindFluent<TEntity, TEntity> Sort<TEntity>(
            this IFindFluent<TEntity, TEntity> query,
            SortRequestCollection<TEntity> sortRequests)
        {
            foreach (var sortRequest in sortRequests)
            {
                var @params = new object[] { sortRequest.SortExpression };
                query = (IOrderedFindFluent<TEntity, TEntity>)
                    query.GetSuitableSortMethod(sortRequest.SortDirection).Invoke(query, @params)!;
            }
            return (IOrderedFindFluent<TEntity, TEntity>)query;
        }

        public static async Task<PagedList<TResult>> ToPagedListAsync<TEntity, TResult>(
            this IFindFluent<TEntity, TResult> query,
            IMongoCollection<TEntity> collection,
            PaginationRequest paginationRequest)
        {
            query.ThrowIfNotOrdered();

            var countFacet = AggregateFacet.Create(
                                InfrastructureConstants.Pagination.COUNT_FACET_NAME,
                                PipelineDefinition<TResult, AggregateCountResult>.Create(
                                    new[]
                                    {
                                        PipelineStageDefinitionBuilder.Count<TResult>()
                                    }
                                )
                            );

            var dataFacet = AggregateFacet.Create(
                                InfrastructureConstants.Pagination.DATA_FACET_NAME,
                                PipelineDefinition<TResult, TResult>.Create(
                                    new PipelineStageDefinition<TResult, TResult>[]
                                    {
                                        PipelineStageDefinitionBuilder.Skip<TResult>(paginationRequest.GetSkipNumber()),
                                        PipelineStageDefinitionBuilder.Limit<TResult>(paginationRequest.PageSize)
                                    }
                                )
                            );

            var aggregation = await collection
                                        .Aggregate()
                                        .Match(query.Filter)
                                        .Project(query.Options.Projection)
                                        .Facet(countFacet, dataFacet)
                                        .ToListAsync();

            var aggregationResults = aggregation.First().Facets;

            var countAggregationResult = aggregationResults
                                            .First(e => e.Name == InfrastructureConstants.Pagination.COUNT_FACET_NAME)
                                            .Output<AggregateCountResult>();

            if (countAggregationResult.Count == 0)
                return PagedList<TResult>.Empty;

            var count = countAggregationResult.Count;

            var data = aggregationResults
                            .First(e => e.Name == InfrastructureConstants.Pagination.DATA_FACET_NAME)
                            .Output<TResult>()
                            .ToArray();

            return new PagedList<TResult>(paginationRequest.PageIndex, paginationRequest.PageSize, data, count);
        }

        private static MethodInfo GetSuitableSortMethod<TEntity>(
            this IFindFluent<TEntity, TEntity> query,
            SortDirection sortDirection)
        {
            var methodNamePrefix = query.IsOrdered() ? "Then" : "Sort";
            var methodNamePostfix = sortDirection.GetPostfixSortMethodName();
            var methodName = $"{methodNamePrefix}By{methodNamePostfix}";
            return typeof(IFindFluentExtensions).GetMethod(methodName)!;
        }

        internal static bool IsOrdered<TEntity, TResult>(this IFindFluent<TEntity, TResult> query)
        {
            return query is IOrderedFindFluent<TEntity, TResult>;
        }
    }
}

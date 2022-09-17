using System.Linq;
using System;
using Infrastructure.Constants;
using MongoDB.Driver;

namespace Infrastructure.Extensions
{
    internal static class QueryExtensions
    {
        internal static void ThrowIfNotOrdered<TEntity>(this IQueryable<TEntity> query)
        {
            if (!query.IsOrdered())
                throw new InvalidOperationException(InfrastructureConstants.ErrorMessages.QUERY_NOT_ORDERED);
        }

        internal static void ThrowIfNotOrdered<TEntity, TResult>(this IFindFluent<TEntity, TResult> query)
        {
            if (!query.IsOrdered())
                throw new InvalidOperationException(InfrastructureConstants.ErrorMessages.QUERY_NOT_ORDERED);
        }
    }
}

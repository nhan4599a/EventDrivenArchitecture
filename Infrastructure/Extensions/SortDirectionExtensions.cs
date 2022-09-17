using Infrastructure.Models;

namespace Infrastructure.Extensions
{
    internal static class SortDirectionExtensions
    {
        internal static string GetPostfixSortMethodName(this SortDirection sortDirection)
        {
            return sortDirection == SortDirection.Ascending ? "" : "Descending";
        }
    }
}

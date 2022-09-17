using Infrastructure.Models;

namespace Infrastructure.Extensions
{
    internal static class PaginationRequestExtensions
    {
        internal static int GetSkipNumber(this PaginationRequest paginationRequest)
        {
            return (paginationRequest.PageIndex - 1) * paginationRequest.PageSize;
        }
    }
}

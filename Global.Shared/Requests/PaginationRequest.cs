namespace Infrastructure.Models
{
    public class PaginationRequest
    {
        public int PageSize { get; private set; }

        public int PageIndex { get; private set; }

        internal PaginationRequest(int pageSize, int pageIndex)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
        }
    }
}

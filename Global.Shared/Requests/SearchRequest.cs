using System;
using System.Linq.Expressions;

namespace Infrastructure.Models
{
    public class SearchRequest<TEntity>
    {
        public bool IsFullEntitiesQuery => Predicate == null;

        public Expression<Func<TEntity, bool>>? Predicate { get; set; }

        public bool IgnoreSoftDelete { get; internal set; } = true;

        public bool UsePagination => PaginationRequest != null;

        public PaginationRequest? PaginationRequest { get; internal set; }

        public bool UseSort => SortRequests != null && !SortRequests.IsEmpty;

        public SortRequestCollection<TEntity>? SortRequests { get; internal set; }
    }

    public class SearchRequestBuilder<TEntity>
    {
        private Expression<Func<TEntity, bool>>? _predicate;

        private SortRequestCollection<TEntity>? _sortRequests;

        private int? _pageSize, _pageIndex;

        private bool _ignoreSoftDelete = true;

        public SearchRequestBuilder<TEntity> Criterial(Expression<Func<TEntity, bool>> predicate)
        {
            _predicate = predicate;
            return this;
        }

        public SearchRequestBuilder<TEntity> Page(uint pageIndex, uint pageSize)
        {
            if (pageIndex == 0 || pageSize == 0)
                throw new Exception();

            _pageIndex = (int)pageIndex;
            _pageSize = (int)pageSize;
            return this;
        }

        public SearchRequestBuilder<TEntity> IncludeSoftDelete()
        {
            _ignoreSoftDelete = false;
            return this;
        }

        public SearchRequestBuilder<TEntity> Ascending(Expression<Func<TEntity, object>> expression)
        {
            _sortRequests ??= new SortRequestCollection<TEntity>();
            _sortRequests.Add(expression);
            return this;
        }

        public SearchRequestBuilder<TEntity> Descending(Expression<Func<TEntity, object>> expression)
        {
            _sortRequests ??= new SortRequestCollection<TEntity>();
            _sortRequests.Add(expression, SortDirection.Descending);
            return this;
        }

        public SearchRequest<TEntity> Build()
        {
            var paginationRequest = _pageSize.HasValue ? new PaginationRequest(_pageSize!.Value, _pageIndex!.Value) : null;
            return new SearchRequest<TEntity>
            {
                Predicate = _predicate,
                IgnoreSoftDelete = _ignoreSoftDelete,
                PaginationRequest = paginationRequest
            };
        }
    }
}

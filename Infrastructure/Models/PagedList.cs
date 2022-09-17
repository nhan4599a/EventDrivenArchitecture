using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Models
{
    public class PagedList<TEntity> : IReadOnlyCollection<TEntity>
    {
        public int Count => Data.Length;

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public bool IsFirstPage { get; private set; }

        public bool IsLastPage { get; private set; }

        public TEntity[] Data { get; private set; }

        public int TotalItemsCount { get; private set; }

        public int MaxPageIndex { get; private set; }

        internal PagedList(int pageIndex, int pageSize, TEntity[] data, int totalItemsCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Data = data;
            TotalItemsCount = totalItemsCount;

            if (PageSize != 0)
            {
                MaxPageIndex = (int)MathF.Ceiling(TotalItemsCount / PageSize);
            }

            IsFirstPage = pageIndex == 1;
            IsLastPage = PageIndex == MaxPageIndex;
        }

        public IEnumerator<TEntity> GetEnumerator() => Data.ToList().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static PagedList<TEntity> Empty => new(0, 0, Array.Empty<TEntity>(), 0);
    }
}

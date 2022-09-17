using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Models
{
    public class SortRequest<TEntity>
    {
        public Expression<Func<TEntity, object>> SortExpression { get; set; }

        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        internal SortRequest(Expression<Func<TEntity, object>> memberExpression, SortDirection sortDirection)
        {
            SortExpression = memberExpression;
            SortDirection = sortDirection;
        }
    }

    public class SortRequestCollection<TEntity> : IEnumerable<SortRequest<TEntity>>
    {
        private readonly Queue<SortRequest<TEntity>> _underlyingCollection;

        public bool IsEmpty { get; private set; }

        public SortRequestCollection()
        {
            _underlyingCollection = new Queue<SortRequest<TEntity>>();
            IsEmpty = true;
        }

        public SortRequestCollection<TEntity> Add(
            Expression<Func<TEntity, object>> expression,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            var sortRequest = new SortRequest<TEntity>(expression, sortDirection);
            _underlyingCollection.Enqueue(sortRequest);
            IsEmpty = false;
            return this;
        }

        public IEnumerator<SortRequest<TEntity>> GetEnumerator()
        {
            return _underlyingCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _underlyingCollection.GetEnumerator();
        }
    }

    public enum SortDirection : byte
    {
        Ascending,
        Descending
    }
}

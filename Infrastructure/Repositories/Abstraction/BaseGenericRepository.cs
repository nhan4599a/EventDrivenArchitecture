using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Models.Abstraction;
using Infrastructure.Models.Relational;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Abstraction
{
    public abstract class BaseGenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
    {
        private readonly bool _isSoftDeleteSupported;

        public bool IsSoftDeleteSupported => _isSoftDeleteSupported;

        public abstract Task<IEnumerable<TEntity>> FindAsync(SearchRequest<TEntity> searchRequest);

        public abstract Task<IEnumerable<TMappedEntity>> FindAsync<TMappedEntity>(SearchRequest<TEntity> searchRequest)
            where TMappedEntity : BaseDataTransferObject<TEntity, TMappedEntity, TKey>;

        public abstract Task<TEntity?> FindByIdAsync(TKey id, bool ignoreSoftDelete = true);

        public abstract Task<TMappedEntity?> FindByIdAsync<TMappedEntity>(TKey id, bool ignoreSoftDelete = true)
            where TMappedEntity : BaseDataTransferObject<TEntity, TMappedEntity, TKey>;

        public abstract Task Add(TEntity entity);

        public abstract Task Update(TEntity entity);

        public abstract Task Delete(TEntity entity);

        public BaseGenericRepository()
        {
            _isSoftDeleteSupported = typeof(TEntity) == typeof(ISoftDeleteEntity);
        }

        protected void ValidateFindRequest(bool ignoreSoftDelete)
        {
            if (!IsSoftDeleteSupported && !ignoreSoftDelete)
                throw new InvalidOperationException();
        }
    }
}

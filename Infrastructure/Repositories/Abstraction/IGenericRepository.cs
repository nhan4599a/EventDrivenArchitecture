using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Models.Relational;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Abstraction
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        bool IsSoftDeleteSupported { get; }

        Task<IEnumerable<TEntity>> FindAsync(SearchRequest<TEntity> searchRequest);

        Task<IEnumerable<TMappedEntity>> FindAsync<TMappedEntity>(SearchRequest<TEntity> searchRequest)
            where TMappedEntity : BaseDataTransferObject<TEntity, TMappedEntity, TKey>;

        Task<TEntity?> FindByIdAsync(TKey id, bool ignoreSoftDelete = true);

        Task<TMappedEntity?> FindByIdAsync<TMappedEntity>(TKey id, bool ignoreSoftDelete)
            where TMappedEntity : BaseDataTransferObject<TEntity, TMappedEntity, TKey>;

        Task Add(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(TEntity entity);
    }
}

using Infrastructure.DataAccess.Abstraction;
using Infrastructure.Extensions;
using Infrastructure.Models;
using Infrastructure.Models.Relational;
using Infrastructure.Repositories.Abstraction;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public abstract class RelationalDbRepository<TEntity, TKey>
        : BaseGenericRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : struct
    {
        protected readonly DbSet<TEntity> Entities;

        public RelationalDbRepository(IRelationalDbContext relationalDbContext)
        {
            Entities = relationalDbContext.Set<TEntity>();
        }

        public override async Task<IEnumerable<TEntity>> FindAsync(SearchRequest<TEntity> searchRequest)
        {
            return await RelationalDbRepository<TEntity, TKey>.GetResultAsync(ApplySearchRequest(searchRequest), searchRequest);
        }

        public override async Task<IEnumerable<TMappedEntity>> FindAsync<TMappedEntity>(SearchRequest<TEntity> searchRequest)
        {
            IQueryable<TMappedEntity> query = ApplySearchRequest(searchRequest).ProjectToType<TMappedEntity>();
            return await RelationalDbRepository<TEntity, TKey>.GetResultAsync(query, searchRequest);
        }

        public override Task<TEntity?> FindByIdAsync(TKey id, bool ignoreSoftDelete = true)
        {
            ValidateFindRequest(ignoreSoftDelete);

            IQueryable<TEntity> query = ApplySearchRequest(ignoreSoftDelete);

            return query.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public override Task<TMappedEntity?> FindByIdAsync<TMappedEntity>(TKey id, bool ignoreSoftDelete = true)
            where TMappedEntity : class
        {
            ValidateFindRequest(ignoreSoftDelete);

            IQueryable<TMappedEntity> query = ApplySearchRequest(ignoreSoftDelete).ProjectToType<TMappedEntity>();

            return query.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public override Task Add(TEntity entity)
        {
            Entities.Add(entity);
            return Task.CompletedTask;
        }

        public override Task Update(TEntity entity)
        {
            Entities.Update(entity);
            return Task.CompletedTask;
        }

        public override Task Delete(TEntity entity)
        {
            Entities.Remove(entity);
            return Task.CompletedTask;
        }

        private IQueryable<TEntity> ApplySearchRequest(SearchRequest<TEntity> searchRequest)
        {
            IQueryable<TEntity> query = ApplySearchRequest(searchRequest.IgnoreSoftDelete);
            if (!searchRequest.IsFullEntitiesQuery)
                query = query.Where(searchRequest.Predicate!);
            if (searchRequest.UseSort)
                query = query.Sort(searchRequest.SortRequests!);
            return query;
        }

        private IQueryable<TEntity> ApplySearchRequest(bool ignoreSoftDelete)
        {
            IQueryable<TEntity> query = Entities;
            if (!ignoreSoftDelete)
                query = query.IgnoreQueryFilters();
            return query;
        }

        private static async Task<IEnumerable<TResult>> GetResultAsync<TResult>(
            IQueryable<TResult> query,
            SearchRequest<TEntity> searchRequest)
            where TResult : class
        {
            if (searchRequest.UsePagination)
                return await query.ToPagedListAsync(searchRequest.PaginationRequest!);
            return await query.ToListAsync();
        }
    }
}

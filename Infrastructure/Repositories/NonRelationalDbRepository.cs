using Global.Shared.Extensions;
using Infrastructure.DataAccess.Abstraction;
using Infrastructure.Extensions;
using Infrastructure.Models;
using Infrastructure.Models.Abstraction;
using Infrastructure.Models.NonRelational;
using Infrastructure.Repositories.Abstraction;
using Mapster;
using Mapster.Models;
using MapsterMapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public abstract class NonRelationalDbRepository<TEntity>
        : BaseGenericRepository<TEntity, string>
        where TEntity : NonRelationDbBaseEntity
    {
        protected IMongoCollection<TEntity> Entities;

        protected IMapper Mapper;

        public NonRelationalDbRepository(INonRelationalDbContext nonRelationalDbContext, IMapper mapper)
        {
            Entities = nonRelationalDbContext.Collection<TEntity>();
            Mapper = mapper;
        }

        public override async Task<IEnumerable<TEntity>> FindAsync(SearchRequest<TEntity> searchRequest)
        {
            ValidateFindRequest(searchRequest.IgnoreSoftDelete);

            var query = ApplySearchRequest(searchRequest);

            return await GetResultAsync(query, searchRequest);
        }

        public override async Task<IEnumerable<TMappedEntity>> FindAsync<TMappedEntity>(SearchRequest<TEntity> searchRequest)
        {
            ValidateFindRequest(searchRequest.IgnoreSoftDelete);

            var mapExpression = GetMapExpression<TMappedEntity>();

            var query = ApplySearchRequest(searchRequest).Project(mapExpression);

            return await GetResultAsync(query, searchRequest);
        }

        public override async Task<TEntity?> FindByIdAsync(string id, bool ignoreSoftDelete = true)
        {
            ValidateFindRequest(ignoreSoftDelete);

            var query = ApplySearchRequest(id, ignoreSoftDelete);

            return await query.FirstOrDefaultAsync();
        }

        public override async Task<TMappedEntity?> FindByIdAsync<TMappedEntity>(string id, bool ignoreSoftDelete = true)
            where TMappedEntity : class
        {
            ValidateFindRequest(ignoreSoftDelete);

            var mapExpression = GetMapExpression<TMappedEntity>();

            var query = ApplySearchRequest(id, ignoreSoftDelete).Project(mapExpression);

            return await query.FirstOrDefaultAsync();
        }

        public override async Task Add(TEntity entity)
        {
            await Entities.InsertOneAsync(entity);
        }

        public override async Task Update(TEntity entity)
        {
            await Entities.FindOneAndReplaceAsync(e => e.Id.Equals(entity.Id), entity);
        }

        public override async Task Delete(TEntity entity)
        {
            await Entities.FindOneAndDeleteAsync(e => e.Id.Equals(entity.Id));
        }

        private IFindFluent<TEntity, TEntity> ApplySearchRequest(SearchRequest<TEntity> searchRequest)
        {
            Expression<Func<TEntity, bool>> expression = e => true;

            if (IsSoftDeleteSupported)
                expression = expression.And(e => (e as ISoftDeleteEntity)!.IsDeleted != searchRequest.IgnoreSoftDelete);

            if (!searchRequest.IsFullEntitiesQuery)
                expression = expression.And(searchRequest.Predicate!);

            var query = Entities.Find(expression);

            if (searchRequest.UseSort)
                query = query.Sort(searchRequest.SortRequests!);

            return query;
        }

        private IFindFluent<TEntity, TEntity> ApplySearchRequest(string id, bool ignoreSoftDelete)
        {
            Expression<Func<TEntity, bool>> expression = (TEntity e) => e.Id.Equals(id);

            if (IsSoftDeleteSupported)
            {
                expression = expression.And(e => (e as ISoftDeleteEntity)!.IsDeleted != ignoreSoftDelete);
            }

            return Entities.Find(expression);
        }

        private async Task<IEnumerable<TResult>> GetResultAsync<TResult>(
            IFindFluent<TEntity, TResult> query, SearchRequest<TEntity> searchRequest)
        {
            if (searchRequest.UsePagination)
                return await query.ToPagedListAsync(Entities, searchRequest.PaginationRequest!);
            return await query.ToListAsync();
        }

        private Expression<Func<TEntity, TMappedEntity>> GetMapExpression<TMappedEntity>()
        {
            var typeTuple = new TypeTuple(typeof(TEntity), typeof(TMappedEntity));
            var expression = Mapper.Config.CreateMapExpression(typeTuple, MapType.Projection);

            return (Expression<Func<TEntity, TMappedEntity>>)expression;
        }
    }
}

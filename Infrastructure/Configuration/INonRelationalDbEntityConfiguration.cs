using MongoDB.Driver;

namespace Infrastructure.Configuration
{
    public interface INonRelationalDbEntityConfiguration<TEntity>
    {
        void Configure(IMongoCollection<TEntity> collection);
    }
}

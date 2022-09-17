using MongoDB.Driver;
using System;

namespace Infrastructure.DataAccess.Abstraction
{
    public interface INonRelationalDbContext : IDisposable
    {
        IMongoClient Client { get; }

        IMongoDatabase Database { get; }

        IClientSessionHandle Session { get; }

        IMongoCollection<TEntity> Collection<TEntity>();
    }
}

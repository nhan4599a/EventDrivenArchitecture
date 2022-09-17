using CatalogService.Shared.Models;
using Infrastructure.DataAccess.Abstraction;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.DataAccess.DbContexts.Abstraction
{
    public interface ICatalogDbContext : INonRelationalDbContext
    {
        IMongoCollection<Product> Products { get; set; }
    }
}

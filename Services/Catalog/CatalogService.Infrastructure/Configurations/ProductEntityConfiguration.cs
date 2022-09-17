using CatalogService.Shared.Models;
using Infrastructure.Configuration;
using MongoDB.Driver;

namespace CatalogService.Infrastructure.Configurations
{
    public class ProductEntityConfiguration : INonRelationalDbEntityConfiguration<Product>
    {
        public void Configure(IMongoCollection<Product> collection)
        {
            var index = new CreateIndexModel<Product>(
                Builders<Product>.IndexKeys.Combine(
                    Builders<Product>.IndexKeys.Text(e => e.Name),
                    Builders<Product>.IndexKeys.Text(e => e.CategoryName),
                    Builders<Product>.IndexKeys.Text(e => e.Description)
                )
            );
            collection.Indexes.CreateOne(index);
        }
    }
}

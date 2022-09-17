using CatalogService.Infrastructure.DataAccess.DbContexts.Abstraction;
using CatalogService.Shared.Models;
using Global.Shared.Settings;
using Infrastructure.DataAccess.DbContexts;
using MongoDB.Driver;
using System.Reflection;

namespace CatalogService.Infrastructure.DataAccess.DbContexts
{
    public class CatalogDbContext : NonRelationalDbContext, ICatalogDbContext
    {
        public IMongoCollection<Product> Products { get; set; } = default!;

        protected override Assembly MigrationAssembly => Assembly.GetExecutingAssembly();

        public CatalogDbContext(NonRelationalDatabaseSetting databaseSetting) : base(databaseSetting)
        {
        }
    }
}

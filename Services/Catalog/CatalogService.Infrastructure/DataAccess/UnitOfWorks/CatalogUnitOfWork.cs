using CatalogService.Infrastructure.DataAccess.DbContexts.Abstraction;
using CatalogService.Infrastructure.Repositories.Abstraction;
using CatalogService.Infrastructure.UnitOfWorks.Abstraction;
using Infrastructure.DataAccess.UnitOfWorks;

namespace CatalogService.Infrastructure.UnitOfWorks
{
    public class CatalogUnitOfWork : NonRelationalDbUnitOfWork, ICatalogUnitOfWork
    {
        public IProductRepository ProductRepository { get; }

        public CatalogUnitOfWork(
            ICatalogDbContext dbContext,
            IProductRepository productRepository) : base(dbContext)
        {
            ProductRepository = productRepository;
        }
    }
}

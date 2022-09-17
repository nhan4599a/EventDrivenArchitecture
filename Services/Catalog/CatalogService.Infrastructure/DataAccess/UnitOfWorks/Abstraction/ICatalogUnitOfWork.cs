using CatalogService.Infrastructure.Repositories.Abstraction;
using Infrastructure.DataAccess.UnitOfWorks.Abstraction;

namespace CatalogService.Infrastructure.UnitOfWorks.Abstraction
{
    public interface ICatalogUnitOfWork : IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
    }
}

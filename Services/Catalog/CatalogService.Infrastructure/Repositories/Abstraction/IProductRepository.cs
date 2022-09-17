using CatalogService.Shared.DTOs;
using CatalogService.Shared.Models;
using Infrastructure.Repositories.Abstraction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogService.Infrastructure.Repositories.Abstraction
{
    public interface IProductRepository : IGenericRepository<Product, string>
    {
        Task<IEnumerable<ProductDataTransferObject>> FindAllProductsAsync(uint pageIndex, uint pageSize);

        Task<IEnumerable<ProductDataTransferObject>> FindProductByCategoryAsync(uint pageIndex, uint pageSize, string categoryName);

        Task<IEnumerable<ProductDataTransferObject>> FindProductByNameAsync(uint pageIndex, uint pageSize, string productName);
    }
}

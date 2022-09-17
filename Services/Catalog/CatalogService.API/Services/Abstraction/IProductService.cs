using CatalogService.Shared.DTOs;
using CatalogService.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogService.API.Services.Abstraction
{
    public interface IProductService
    {
        Task<Product?> FindProductById(string productId);

        Task<IEnumerable<ProductDataTransferObject>> FindProductsByNameAsync(uint pageIndex, uint pageSize, string productName);

        Task<IEnumerable<ProductDataTransferObject>> FindProductsByCategoryAsync(uint pageIndex, uint pageSize, string categoryName);

        Task<bool> DeleteProductAsync(string productId);

        Task<string> CreateProductAsync(Product product);
    }
}

using CatalogService.Infrastructure.DataAccess.DbContexts.Abstraction;
using CatalogService.Infrastructure.Repositories.Abstraction;
using CatalogService.Shared.DTOs;
using CatalogService.Shared.Models;
using Infrastructure.Models;
using Infrastructure.Repositories;
using MapsterMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogService.Infrastructure.Repositories
{
    public class ProductRepository : NonRelationalDbRepository<Product>, IProductRepository
    {
        public ProductRepository(ICatalogDbContext nonRelationalDbContext, IMapper mapper)
            : base(nonRelationalDbContext, mapper)
        {
        }

        public Task<IEnumerable<ProductDataTransferObject>> FindAllProductsAsync(uint pageIndex, uint pageSize)
        {
            var searchRequest = new SearchRequestBuilder<Product>()
                                        .Page(pageIndex, pageSize)
                                        .Ascending(e => e.Id)
                                        .Build();
            return FindAsync<ProductDataTransferObject>(searchRequest);
        }

        public Task<IEnumerable<ProductDataTransferObject>> FindProductByCategoryAsync(
            uint pageIndex,
            uint pageSize,
            string categoryName)
        {
            var searchRequest = new SearchRequestBuilder<Product>()
                                        .Criterial(e => e.CategoryName.Contains(categoryName))
                                        .Page(pageIndex, pageSize)
                                        .Ascending(e => e.Id)
                                        .Build();
            return FindAsync<ProductDataTransferObject>(searchRequest);
        }

        public Task<IEnumerable<ProductDataTransferObject>> FindProductByNameAsync(
            uint pageIndex,
            uint pageSize,
            string productName)
        {
            var searchRequest = new SearchRequestBuilder<Product>()
                                        .Criterial(e => e.Name.Contains(productName))
                                        .Page(pageIndex, pageSize)
                                        .Ascending(e => e.Id)
                                        .Build();
            return FindAsync<ProductDataTransferObject>(searchRequest);
        }
    }
}

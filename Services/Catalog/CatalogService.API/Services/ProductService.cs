using CatalogService.API.Services.Abstraction;
using CatalogService.Infrastructure.UnitOfWorks.Abstraction;
using CatalogService.Shared.DTOs;
using CatalogService.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogService.API.Services
{
    public class ProductService : IProductService
    {
        private readonly ICatalogUnitOfWork _unitOfWork;

        public ProductService(ICatalogUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Product?> FindProductById(string productId)
        {
            return _unitOfWork.ProductRepository.FindByIdAsync(productId);
        }

        public Task<IEnumerable<ProductDataTransferObject>> FindProductsByCategoryAsync(
            uint pageIndex,
            uint pageSize,
            string categoryName)
        {
            return _unitOfWork.ProductRepository.FindProductByCategoryAsync(pageIndex, pageSize, categoryName);
        }

        public Task<IEnumerable<ProductDataTransferObject>> FindProductsByNameAsync(
            uint pageIndex,
            uint pageSize,
            string productName)
        {
            return _unitOfWork.ProductRepository.FindProductByNameAsync(pageIndex, pageSize, productName);
        }

        public async Task<string> CreateProductAsync(Product product)
        {
            await _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.SaveChangesAsync();
            return product.Id;
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            var product = await _unitOfWork.ProductRepository.FindByIdAsync(productId);
            if (product == null)
                throw new Exception();
            await _unitOfWork.ProductRepository.Delete(product);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}

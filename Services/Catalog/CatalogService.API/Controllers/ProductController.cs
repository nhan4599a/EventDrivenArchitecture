using CatalogService.API.Services.Abstraction;
using CatalogService.Shared.DTOs;
using Infrastructure.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogService.API.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService) : base(logger)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDataTransferObject>> Index()
        {
            return await _productService.FindProductsByNameAsync(1, 10, "");
        }
    }
}
using CatalogService.Shared.Models;
using Infrastructure.DTOs;

namespace CatalogService.Shared.DTOs
{
    public class ProductDataTransferObject : BaseDataTransferObject<Product, ProductDataTransferObject, string>
    {
        public string ProductName { get; set; } = default!;

        public string ProductDescription { get; set; } = default!;

        public string CategoryName { get; set; } = default!;

        public double Price { get; set; }

        public string[]? Images { get; set; }

        protected override void AddCustomMappings()
        {
            SetCustomMappings()
                .Map(dest => dest.ProductName, source => source.Name)
                .Map(dest => dest.ProductDescription, source => source.Description);
        }
    }
}

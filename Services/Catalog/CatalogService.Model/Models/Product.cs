using Infrastructure.Models.NonRelational;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.Shared.Models
{
    public class Product : NonRelationDbBaseEntity
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public double Price { get; set; }

        [BsonElement("category")]
        public string CategoryName { get; set; } = null!;

        public string[]? Images { get; set; }
    }
}

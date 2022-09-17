using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Infrastructure.Models.Relational
{
    public abstract class BaseEntity<TKey>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public TKey Id { get; set; } = default!;

        public DateTimeOffset CreatedDate { get; set; }
    }
}

using Infrastructure.DTOs.Abstraction;
using Infrastructure.Models.Relational;
using Mapster;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.DTOs
{
    public abstract class BaseDataTransferObject<TEntity, TDataTransfer, TKey>
        : IRegister, IDataTransferObject<TEntity>
        where TEntity : BaseEntity<TKey>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public TKey Id { get; set; } = default!;

        private TypeAdapterConfig Config { get; set; } = default!;

        protected virtual void AddCustomMappings() { }

        protected TypeAdapterSetter<TEntity, TDataTransfer> SetCustomMappings()
        {
            return Config.ForType<TEntity, TDataTransfer>();
        }

        protected TypeAdapterSetter<TDataTransfer, TEntity> SetCustomMappingsReverse()
        {
            return Config.ForType<TDataTransfer, TEntity>();
        }

        public void Register(TypeAdapterConfig config)
        {
            Config = config;
            AddCustomMappings();
        }

        public TEntity ToEntity()
        {
            return this.Adapt<TEntity>();
        }

        public TEntity ToEntity(TEntity entity)
        {
            return this.Adapt(entity);
        }

        public static TDataTransfer FromEntity(TEntity entity)
        {
            return entity.Adapt<TDataTransfer>();
        }
    }
}

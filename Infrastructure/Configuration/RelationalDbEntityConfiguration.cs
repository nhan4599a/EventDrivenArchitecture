using Infrastructure.Models.Abstraction;
using Infrastructure.Models.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class RelationalDbBaseEntityConfiguration<TEntity, TKey>
        : IEntityTypeConfiguration<BaseEntity<TKey>>
        where TEntity : BaseEntity<TKey>
        where TKey : struct
    {
        public virtual void Configure(EntityTypeBuilder<BaseEntity<TKey>> builder)
        {
            if (typeof(TEntity) == typeof(ISoftDeleteEntity))
            {
                builder.HasQueryFilter(e => !((ISoftDeleteEntity)e).IsDeleted);
            }
        }
    }
}

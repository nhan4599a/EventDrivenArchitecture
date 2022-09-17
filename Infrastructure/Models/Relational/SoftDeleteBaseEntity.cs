using Infrastructure.Models.Abstraction;

namespace Infrastructure.Models.Relational
{
    public abstract class SoftDeleteBaseEntity<TKey> : BaseEntity<TKey>, ISoftDeleteEntity
    {
        public bool IsDeleted { get; set; }
    }
}

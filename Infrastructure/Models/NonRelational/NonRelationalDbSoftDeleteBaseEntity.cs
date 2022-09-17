using Infrastructure.Models.Abstraction;

namespace Infrastructure.Models.NonRelational
{
    public class NonRelationalDbSoftDeleteBaseEntity : NonRelationDbBaseEntity, ISoftDeleteEntity
    {
        public bool IsDeleted { get; set; }
    }
}

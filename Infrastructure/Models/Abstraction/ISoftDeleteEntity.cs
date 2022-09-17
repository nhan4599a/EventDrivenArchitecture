namespace Infrastructure.Models.Abstraction
{
    public interface ISoftDeleteEntity
    {
        bool IsDeleted { get; set; }
    }
}

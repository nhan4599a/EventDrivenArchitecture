namespace Infrastructure.DTOs.Abstraction
{
    public interface IDataTransferObject<TEntity>
    {
        TEntity ToEntity();

        TEntity ToEntity(TEntity entity);
    }
}

using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.UnitOfWorks.Abstraction
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync(int? expectedAffectedRows = null, CancellationToken cancellationToken = default);
    }
}

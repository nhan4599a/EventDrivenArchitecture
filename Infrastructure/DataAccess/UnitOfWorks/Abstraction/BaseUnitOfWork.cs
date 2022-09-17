using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.UnitOfWorks.Abstraction
{
    public abstract class BaseUnitOfWork : IUnitOfWork
    {
        public abstract Task<bool> SaveChangesAsync(
            int? expectedAffectedRows = null,
            CancellationToken cancellationToken = default);

        protected static bool CheckExpectedRows(int? expectedAffectedRows, int actualAffectedRows)
        {
            if (expectedAffectedRows.HasValue)
            {
                return actualAffectedRows == expectedAffectedRows.Value;
            }

            return true;
        }
    }
}

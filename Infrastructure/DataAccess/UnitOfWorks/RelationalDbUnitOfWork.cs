using Infrastructure.DataAccess.Abstraction;
using Infrastructure.DataAccess.UnitOfWorks.Abstraction;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.UnitOfWorks
{
    public class RelationalDbUnitOfWork : BaseUnitOfWork
    {
        private readonly IRelationalDbContext _dbContext;

        public RelationalDbUnitOfWork(IRelationalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<bool> SaveChangesAsync(
            int? expectedAffectedRows = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var actualAffectedRows = await _dbContext.SaveChangesAsync(cancellationToken);
                return CheckExpectedRows(expectedAffectedRows, actualAffectedRows);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

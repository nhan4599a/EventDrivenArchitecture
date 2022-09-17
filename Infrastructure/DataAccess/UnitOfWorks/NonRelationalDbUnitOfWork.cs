using Infrastructure.DataAccess.Abstraction;
using Infrastructure.DataAccess.UnitOfWorks.Abstraction;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.UnitOfWorks
{
    public class NonRelationalDbUnitOfWork : BaseUnitOfWork
    {
        private readonly INonRelationalDbContext _dbContext;

        public NonRelationalDbUnitOfWork(INonRelationalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<bool> SaveChangesAsync(
            int? expectedAffectedRows = null, CancellationToken cancellationToken = default)
        {
            await _dbContext.Session.CommitTransactionAsync(cancellationToken);
            var actualAffectedRows = await _dbContext.Database.GetAffectedRowsAsync(cancellationToken);
            return CheckExpectedRows(expectedAffectedRows, actualAffectedRows);
        }
    }

    internal static class NonRelationalDbExtensions
    {
        public async static Task<int> GetAffectedRowsAsync(
            this IMongoDatabase database, CancellationToken cancellationToken = default)
        {
            var commandResult = await database.RunCommandAsync<BsonDocument>(
                                                                "{\"getLastError\":1}",
                                                                cancellationToken: cancellationToken);
            return commandResult["n"].AsInt32;
        }
    }
}

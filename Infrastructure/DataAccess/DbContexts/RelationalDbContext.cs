using Infrastructure.DataAccess.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.DbContexts
{
    public abstract class RelationalDbContext : DbContext, IRelationalDbContext
    {
    }
}

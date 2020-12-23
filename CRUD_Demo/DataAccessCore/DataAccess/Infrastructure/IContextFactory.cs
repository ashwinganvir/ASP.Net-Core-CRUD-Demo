using Microsoft.EntityFrameworkCore;

namespace DataAccessCore.DataAccess.Infrastructure
{
    public interface IContextFactory
    {
        TContext Create<TContext>()
            where TContext : DbContext;
    }
}

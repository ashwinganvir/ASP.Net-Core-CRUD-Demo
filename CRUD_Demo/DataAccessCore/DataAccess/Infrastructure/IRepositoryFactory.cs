using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessCore.DataAccess.Infrastructure
{
    public interface IRepositoryFactory
    {
        TRepository Create<TRepository>()
            where TRepository : class;
    }
}

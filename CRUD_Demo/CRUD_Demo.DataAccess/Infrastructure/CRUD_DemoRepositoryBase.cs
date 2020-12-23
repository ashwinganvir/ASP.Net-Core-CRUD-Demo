using CRUD_Demo.DataAccess.Context;

using DataAccessCore.DataAccess.Repository;

namespace CRUD_Demo.DataAccess.Infrastructure
{
    internal abstract class CRUD_DemoRepositoryBase<TEntity> : RepositoryBase<CRUD_DemoContext, TEntity>
        where TEntity : class
    {
        protected CRUD_DemoRepositoryBase(CRUD_DemoContext context)
            : base(context)
        { }
    }
}

using CRUD_Demo.DataAccess.Context;
using CRUD_Demo.DataAccess.Entities;
using CRUD_Demo.DataAccess.Infrastructure;

using DataAccessCore.DataAccess.Infrastructure;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_Demo.DataAccess.Repositories
{
    public interface IContactRepository : IRepositoryAsync<Contact>, IRepository<Contact>
    {
    }

    internal class ContactRepository : CRUD_DemoRepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(CRUD_DemoContext context)
            : base(context)
        { }
    }
}

using AutoMapper;

using CRUD_Demo.Common.Models;
using CRUD_Demo.DataAccess.Repositories;

using DataAccessCore.DataAccess.Infrastructure;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_Demo.Business.Components.Contact
{
    public interface IGetContactComponent
    {
        Task<IEnumerable<ContactModel>> GetAllAsync();
    }

    public class GetContactComponent : IGetContactComponent
    {
        private readonly IRepositoryFactory _repoFactory;
        private readonly IMapper _mapper;

        public GetContactComponent(IRepositoryFactory repoFactory,
                                        IMapper mapper)
        {
            _repoFactory = repoFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContactModel>> GetAllAsync()
        {
            using (var repo = _repoFactory.Create<IContactRepository>())
            {
                var contacts = await repo.GetAllAsync();

                var contactModels = _mapper.Map<IEnumerable<ContactModel>>(contacts);

                return contactModels;
            }
        }
    }
}

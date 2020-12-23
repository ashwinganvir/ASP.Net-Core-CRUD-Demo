using AutoMapper;

using CRUD_Demo.Common.Exceptions;
using CRUD_Demo.Common.Models;
using CRUD_Demo.DataAccess.Repositories;

using DataAccessCore.DataAccess.Infrastructure;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using static CRUD_Demo.Common.Constants.EntityConstants;

using ContactEntity = CRUD_Demo.DataAccess.Entities;

namespace CRUD_Demo.Business.Components.Contact
{
    public interface IDeleteContactComponent
    {
        Task DeleteAsync(Guid contactId);
    }

    public class DeleteContactComponent : IDeleteContactComponent
    {
        private readonly IRepositoryFactory _repoFactory;
        private readonly IMapper _mapper;

        public DeleteContactComponent(IRepositoryFactory repoFactory,
                                        IMapper mapper)
        {
            _repoFactory = repoFactory;
            _mapper = mapper;
        }

        public async Task DeleteAsync(Guid contactId)
        {
            using (var repo = _repoFactory.Create<IContactRepository>())
            {
                ContactEntity.Contact existingContact = repo.GetById(contactId);

                if (existingContact == null)
                {
                    throw new InvalidContactIdException("Invalid ContactId.");
                }

                await repo.DeleteAsync(existingContact);
                repo.SaveChanges();
            }
        }
    }
}

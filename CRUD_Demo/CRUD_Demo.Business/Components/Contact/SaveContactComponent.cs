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
    public interface ISaveContactComponent
    {
        Task<ContactModel> SaveAsync(ContactModel contactModel);
    }

    public class SaveContactComponent : ISaveContactComponent
    {
        private readonly IRepositoryFactory _repoFactory;
        private readonly IMapper _mapper;

        public SaveContactComponent(IRepositoryFactory repoFactory,
                                        IMapper mapper)
        {
            _repoFactory = repoFactory;
            _mapper = mapper;
        }

        public async Task<ContactModel> SaveAsync(ContactModel contactModel)
        {
            return contactModel.ContactId == null
                   ? await CreateAsync(contactModel)
                   : await UpdateAsync(contactModel);
        }

        private async Task<ContactModel> CreateAsync(ContactModel contactModel)
        {
            using (var repo = _repoFactory.Create<IContactRepository>())
            {
                ContactEntity.Contact contact = _mapper.Map<ContactEntity.Contact>(contactModel);

                var savedContact = await repo.AddAsync(contact);
                repo.SaveChanges();

                var savedContactModel = _mapper.Map<ContactModel>(savedContact);

                return savedContactModel;
            }
        }

        private async Task<ContactModel> UpdateAsync(ContactModel contactModel)
        {
            using (var repo = _repoFactory.Create<IContactRepository>())
            {
                ContactEntity.Contact existingContact = repo.GetById(contactModel.ContactId);

                if (existingContact == null)
                {
                    throw new RecordNotFoundException("Invalid Contact details.");
                }

                existingContact.FirstName = contactModel.FirstName;
                existingContact.LastName = contactModel.LastName;
                existingContact.PhoneNumber = contactModel.PhoneNumber;
                existingContact.Email = contactModel.Email;
                existingContact.Status = ((Status)Enum.Parse(typeof(Status), contactModel.Status));

                await repo.UpdateAsync(existingContact);
                repo.SaveChanges();

                return contactModel;
            }
        }        
    }
}

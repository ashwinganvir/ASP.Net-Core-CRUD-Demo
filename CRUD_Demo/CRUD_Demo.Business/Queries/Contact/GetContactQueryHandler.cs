using CRUD_Demo.Business.Components.Contact;
using CRUD_Demo.Business.Infrastructure;
using CRUD_Demo.Common.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUD_Demo.Business.Queries.Contact
{
    internal class GetContactQueryHandler : IQueryHandlerAsync<GetContactQuery, IEnumerable<ContactModel>>
    {
        private readonly IGetContactComponent _getContactComponent;

        public GetContactQueryHandler(IGetContactComponent component)
        {
            _getContactComponent = component;
        }

        public async Task<IEnumerable<ContactModel>> HandleAsync(GetContactQuery query)
        {
            var contactModels = await _getContactComponent.GetAllAsync();

            return contactModels;
        }
    }
}

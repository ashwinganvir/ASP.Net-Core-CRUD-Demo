using CRUD_Demo.Business.Components.Contact;
using CRUD_Demo.Business.Infrastructure;
using CRUD_Demo.Common.Models;

using System.Threading.Tasks;

namespace CRUD_Demo.Business.Commands.Contact
{
    public class SaveContactCommandHandler : ICommandHandlerAsync<SaveContactCommand, ContactModel>
    {
        private readonly ISaveContactComponent _saveContactComponent;

        public SaveContactCommandHandler(ISaveContactComponent component)
        {
            _saveContactComponent = component;
        }

        public async Task<ContactModel> HandleAsync(SaveContactCommand command)
        {
            var contactModel = await _saveContactComponent.SaveAsync(command.ContactModel);

            return contactModel;
        }
    }
}

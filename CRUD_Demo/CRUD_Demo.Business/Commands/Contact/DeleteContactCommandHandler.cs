using CRUD_Demo.Business.Components.Contact;
using CRUD_Demo.Business.Infrastructure;
using CRUD_Demo.Common.Models;

using System.Threading.Tasks;

namespace CRUD_Demo.Business.Commands.Contact
{
    public class DeleteSaveContactCommandHandler : ICommandHandlerAsync<DeleteContactCommand>
    {
        private readonly IDeleteContactComponent _deleteContactComponent;

        public DeleteSaveContactCommandHandler(IDeleteContactComponent component)
        {
            _deleteContactComponent = component;
        }

        public async Task HandleAsync(DeleteContactCommand command)
        {
            await _deleteContactComponent.DeleteAsync(command.ContactId);
        }
    }
}

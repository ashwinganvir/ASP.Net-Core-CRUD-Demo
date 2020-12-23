using CRUD_Demo.Business.Infrastructure;
using CRUD_Demo.Common.Models;

namespace CRUD_Demo.Business.Commands.Contact
{
    public class SaveContactCommand : CommandBase
    {
        public ContactModel ContactModel { get; set; }
    }
}

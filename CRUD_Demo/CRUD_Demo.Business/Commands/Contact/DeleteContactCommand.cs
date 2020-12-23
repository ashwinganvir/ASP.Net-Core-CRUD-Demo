using CRUD_Demo.Business.Infrastructure;
using CRUD_Demo.Common.Models;

using System;

namespace CRUD_Demo.Business.Commands.Contact
{
    public class DeleteContactCommand : CommandBase
    {
        public Guid ContactId { get; set; }
    }
}

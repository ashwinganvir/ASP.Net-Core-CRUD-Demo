using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_Demo.Common.Models
{
    public class ContactModel
    {
        public Guid ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public decimal PhoneNumber { get; set; }
        public string Status { get; set; }
    }
}

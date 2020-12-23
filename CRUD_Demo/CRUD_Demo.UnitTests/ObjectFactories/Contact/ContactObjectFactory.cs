using CRUD_Demo.Common.Models;

using System;
using System.Collections.Generic;
using System.Text;

using ContactEntities = CRUD_Demo.DataAccess.Entities;

namespace CRUD_Demo.UnitTests.ObjectFactories.Contact
{
    internal static class ContactObjectFactory
    {
        public static IEnumerable<ContactEntities.Contact> GetEntityList()
        {
            return new List<ContactEntities.Contact>
            {
                new ContactEntities.Contact
                {
                    ContactId = TestingConstants.Identifier.Unique.Expected,
                    FirstName = "XYZ",
                    LastName = "ABC",
                    Email = "xyz@gmail.com",
                    PhoneNumber = 000000000
                }
            };
        }

        public static IEnumerable<ContactModel> GetModelList()
        {
            return new List<ContactModel>
            {
                new ContactModel
                {
                    ContactId = TestingConstants.Identifier.Unique.Expected,
                    FirstName = "XYZ",
                    LastName = "ABC",
                    Email = "xyz@gmail.com",
                    PhoneNumber = 000000000
                }
            };
        }
    }
}

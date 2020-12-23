using AutoMapper;

using CRUD_Demo.Common.Models;
using CRUD_Demo.DataAccess.Entities;

using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_Demo.Business.MappingProfiles
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap<Contact, ContactModel>()
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .ReverseMap()
               .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}

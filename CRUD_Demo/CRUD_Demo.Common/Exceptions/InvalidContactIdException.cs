using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_Demo.Common.Exceptions
{
    public class InvalidContactIdException : Exception
    {
        public InvalidContactIdException()
        { }

        public InvalidContactIdException(string message)
            : base(message) { }
    }
}

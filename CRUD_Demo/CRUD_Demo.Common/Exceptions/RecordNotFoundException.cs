using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_Demo.Common.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException()
        { }

        public RecordNotFoundException(string message)
            : base(message) { }
    }
}

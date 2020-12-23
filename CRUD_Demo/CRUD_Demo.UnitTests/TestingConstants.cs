using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_Demo.UnitTests
{
    internal static class TestingConstants
    {
        internal static class Identifier
        {
            public static class Unique
            {
                public static Guid Expected = new Guid("7f16c9d8-2519-4a7d-af3f-de1a60d71f58");
                public static Guid Invalid = Guid.Empty;
                public static Guid GenerateGuid()
                {
                    return Guid.NewGuid();
                }
            }
        }
    }
}

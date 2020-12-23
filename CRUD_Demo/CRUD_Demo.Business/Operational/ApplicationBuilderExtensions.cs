using CRUD_Demo.DataAccess.Operational;

using Microsoft.AspNetCore.Builder;

namespace CRUD_Demo.Business.Operational
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseApplication(this IApplicationBuilder app)
        {
            app.TryCreateDatabase();
        }
    }
}

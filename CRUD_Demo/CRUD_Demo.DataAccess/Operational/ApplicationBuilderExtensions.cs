using CRUD_Demo.DataAccess.Context;

using DataAccessCore.DataAccess.Infrastructure;
using DataAccessCore.Database.Operational;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_Demo.DataAccess.Operational
{
    public static class ApplicationBuilderExtensions
    {
        public static void TryCreateDatabase(this IApplicationBuilder app)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var factory = serviceScope.ServiceProvider.GetService<IContextFactory>();

                using (var context = factory.Create<CRUD_DemoContext>())
                {
                    // if the context is null
                    // the connection settings have not been configured
                    if (context == null)
                    {
                        return;
                    }
                }

                var logger = serviceScope.ServiceProvider.GetService<ILogger>();

                const string baseSqlScriptDirectory = "_Sql";

                string[] orderedScripts =
                {
                    "Constraints.sql",
                    "Data.sql",
                    "StoredProcedures.sql"
                };

                try
                {
                    serviceScope.CreateDatabase<CRUD_DemoContext>(baseSqlScriptDirectory, orderedScripts);
                }
                catch (System.Exception ex)
                {
                    logger.Error(ex, "Error occured while creating database.");
                }
            }
        }
    }
}

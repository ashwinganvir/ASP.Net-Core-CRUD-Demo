using CRUD_Demo.Common.Settings.Models;
using CRUD_Demo.DataAccess.Context;

using DataAccessCore.DataAccess.Operational;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CRUD_Demo.DataAccess.Operational
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationSection connectSection = configuration.GetSection("ConnectSettings");
            var settings = connectSection.Get<ConnectModel>();
            string connectionString = settings.ConnectionString;
            services.AddDbContext<CRUD_DemoContext>(builder =>
            {
                builder.UseSqlServer(connectionString);
            });

            string[] repositoryNamespaces =
            {
                    "CRUD_Demo.DataAccess.Repositories"
            };

            var optionsBuilder = new DbContextOptionsBuilder();

            optionsBuilder.UseSqlServer(connectionString);

            services.AddDataAccessCore<CRUD_DemoContext>(optionsBuilder, repositoryNamespaces);
        }
    }
}

using DataAccessCore.DataAccess.Context;
using DataAccessCore.DataAccess.Infrastructure;
using DataAccessCore.DataAccess.Repository;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessCore.DataAccess.Operational
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataAccessCore<TContext>(this IServiceCollection services, DbContextOptionsBuilder optionsBuilder, string repositoryNamespace, params string[] excludedRepositoryNamespaces)
            where TContext : DbContext
        {
            RepositoryFactory.Register<TContext>(optionsBuilder, new[] { repositoryNamespace }, excludedRepositoryNamespaces);

            services.AddSingleton<IContextFactory>(_ => new ContextFactory(optionsBuilder));
            services.AddSingleton<IRepositoryFactory, RepositoryFactory>();
        }

        public static void AddDataAccessCore<TContext>(this IServiceCollection services, DbContextOptionsBuilder optionsBuilder, string[] repositoryNamespaces, params string[] excludedRepositoryNamespaces)
            where TContext : DbContext
        {
            RepositoryFactory.Register<TContext>(optionsBuilder, repositoryNamespaces, excludedRepositoryNamespaces);

            services.AddSingleton<IContextFactory>(_ => new ContextFactory(optionsBuilder));
            services.AddSingleton<IRepositoryFactory, RepositoryFactory>();
        }
    }
}

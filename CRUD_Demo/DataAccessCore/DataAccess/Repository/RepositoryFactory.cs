using DataAccessCore.DataAccess.Context;
using DataAccessCore.DataAccess.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;

namespace DataAccessCore.DataAccess.Repository
{
    internal class RepositoryFactory : IRepositoryFactory
    {
        private static Lazy<ServiceProvider> _serviceProvider;

        public static void Register<TContext>(DbContextOptionsBuilder optionsBuilder, string[] repositoryNamespaces, params string[] excludedNamespaces)
            where TContext : DbContext
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddSingleton<IContextFactory>(_ => new ContextFactory(optionsBuilder));

            var repositories = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsAbstract
                            && !string.IsNullOrWhiteSpace(x.Namespace)
                            && repositoryNamespaces.Any(r => x.Namespace.StartsWith(r)))
                .Where(x => !excludedNamespaces.Any(n => x.Namespace.Equals(n)))
                .Select(x => new
                {
                    Implementation = x,
                    Service = x.GetInterface($"I{x.Name}")
                })
                .Where(x => x.Service != null);

            foreach (var repo in repositories)
            {
                collection.AddTransient(repo.Service, repo.Implementation);
            }

            ServiceProvider serviceProvider = collection.BuildServiceProvider();

            _serviceProvider = new Lazy<ServiceProvider>(serviceProvider);
        }

        public TRepository Create<TRepository>()
            where TRepository : class
        {
            return _serviceProvider.Value.GetService<TRepository>();
        }
    }
}

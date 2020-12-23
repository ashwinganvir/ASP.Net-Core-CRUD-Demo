using AutoMapper;

using CRUD_Demo.Business.Commands;
using CRUD_Demo.Business.Infrastructure;
using CRUD_Demo.Business.MappingProfiles;
using CRUD_Demo.Business.Queries;
using CRUD_Demo.DataAccess.Operational;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;

namespace CRUD_Demo.Business.Operational
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccess(configuration);

            services.AddComponents();

            services.AddCommands();
            services.AddQueries();

            services.AddMappers();
        }

        private static void AddComponents(this IServiceCollection services)
        {
            string[] componentNamespaces =
            {
                "CRUD_Demo.Business.Components"
            };

            // Components
            var repositories = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.FullName.StartsWith("CRUD_Demo.Business"))
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsAbstract
                            && !string.IsNullOrWhiteSpace(x.Namespace)
                            && componentNamespaces.Any(c => x.Namespace.StartsWith(c)))
                .Select(x => new
                {
                    Implementation = x,
                    Service = x.GetInterface($"I{x.Name}")
                })
                .Where(x => x.Service != null);

            foreach (var repo in repositories)
            {
                services.AddTransient(repo.Service, repo.Implementation);
            }
        }

        private static void AddCommands(this IServiceCollection services)
        {
            // Command handlers
            var commands = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsAbstract
                            && !string.IsNullOrWhiteSpace(x.Namespace)
                            && x.Namespace.StartsWith("CRUD_Demo.Business.Commands"));


            Type[] commandTypes =
            {
                typeof(ICommandHandlerAsync<>),
                typeof(ICommandHandlerAsync<,>)
            };

            foreach (var command in commands)
            {
                Type service = command.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType
                                         && commandTypes.Any(c => c == i.GetGenericTypeDefinition()));

                if (service == null)
                {
                    continue;
                }

                services.AddTransient(service, command);
            }

            services.AddSingleton<ICommandHandler>(provider => new CommandHandler(provider));
        }

        public static void AddQueries(this IServiceCollection services)
        {
            // Query handlers
            var queries = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => !x.IsAbstract
                            && !string.IsNullOrWhiteSpace(x.Namespace)
                            && x.Namespace.StartsWith("CRUD_Demo.Business.Queries"));


            Type[] queryTypes =
            {
                typeof(IQueryHandlerAsync<,>)
            };

            foreach (var query in queries)
            {
                Type service = query.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType
                                         && queryTypes.Any(c => c == i.GetGenericTypeDefinition()));

                if (service == null)
                {
                    continue;
                }

                services.AddTransient(service, query);
            }

            services.AddSingleton<IQueryHandler>(provider => new QueryHandler(provider));
        }

        private static void AddMappers(this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<ContactProfile>();
            });

            IMapper mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}

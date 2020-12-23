using DataAccessCore.DataAccess.Infrastructure;
using DataAccessCore.Database.Create;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessCore.Database.Operational
{
    /// <summary>
    /// Provide database creation and migration functionality as an extension of IServiceScope.
    /// </summary>
    public static class ServiceScopeExtensions
    {
        /// <summary>
        /// Creates the database for the given context if it does not already exist.
        /// </summary>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="serviceScope">The service scope the context belongs to.</param>
        public static void CreateDatabase<TContext>(this IServiceScope serviceScope)
            where TContext : DbContext
        {
            var contextFactory = serviceScope.ServiceProvider.GetService<IContextFactory>();

            using (TContext context = contextFactory.Create<TContext>())
            {
                context.Create();
            }
        }

        /// <summary>
        /// Creates the database for the given context if it does not already exist,
        /// and then executes scripts, in order, on the passed context for the given base directory.
        /// </summary>
        /// <para>
        /// The base directory must be a direct child of the application directory.
        /// The ordered scripts must be within the base directory.
        /// </para>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="serviceScope">The service scope the context belongs to.</param>
        /// <param name="baseSqlScriptDirectory">The base directory for scripts.</param>
        /// <param name="orderedSqlScriptFileNames">The scripts to be executed in order they are passed in.</param>
        public static void CreateDatabase<TContext>(this IServiceScope serviceScope, string baseSqlScriptDirectory, params string[] orderedSqlScriptFileNames)
            where TContext : DbContext
        {
            var contextFactory = serviceScope.ServiceProvider.GetService<IContextFactory>();

            using (TContext context = contextFactory.Create<TContext>())
            {
                context.Create();

                context.ExecuteScripts(baseSqlScriptDirectory, orderedSqlScriptFileNames);
            }
        }
    }
}

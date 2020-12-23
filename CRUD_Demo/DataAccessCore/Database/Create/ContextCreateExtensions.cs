using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataAccessCore.Database.Create
{
    /// <summary>
    /// Context extensions for database creation and seeding.
    /// </summary>
    internal static class ContextCreateExtensions
    {
        /// <summary>
        /// Creates the database for the given context if it does not already exist.
        /// </summary>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="context">The context to create.</param>
        public static void Create<TContext>(this TContext context)
            where TContext : DbContext
        {
            context.Database.EnsureCreated();
        }

        /// <summary>
        /// Execute scripts, in order, on the passed context for the given base directory.
        /// </summary>
        /// <para>
        /// The base directory must be a direct child of the application directory.
        /// The ordered scripts must be within the base directory.
        /// </para>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="context">The context to execute scripts on.</param>
        /// <param name="baseSqlScriptDirectory">The base directory for scripts.</param>
        /// <param name="orderedSqlScriptFileNames">The scripts to be executed in order they are passed in.</param>
        public static void ExecuteScripts<TContext>(this TContext context, string baseSqlScriptDirectory, params string[] orderedSqlScriptFileNames)
            where TContext : DbContext
        {
            foreach (string sqlScriptFileName in orderedSqlScriptFileNames)
            {
                context.ExecuteScript(baseSqlScriptDirectory, sqlScriptFileName);
            }
        }

        /// <summary>
        /// Add seed data for the given entity to the given context.
        /// </summary>
        /// <remarks>
        /// This only adds data to the context and returns it. It does not save the data.
        /// </remarks>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="context">The context to add seed data to.</param>
        /// <param name="seedData">Data for the given <see cref="TEntity">Entity</see>.</param>
        /// <returns>The <see cref="TContext"/> with added data.</returns>
        public static TContext AddSeedData<TContext, TEntity>(this TContext context, IEnumerable<TEntity> seedData)
            where TContext : DbContext
            where TEntity : class
        {
            DbSet<TEntity> set = context.Set<TEntity>();
            if (set.Any())
            {
                return context;
            }

            foreach (TEntity entity in seedData)
            {
                set.Add(entity);
            }

            return context;
        }

        /// <summary>
        /// Add seed data for the given entity to the given context, only if that data does not exist.
        /// </summary>
        /// <remarks>
        /// This only adds data to the context and returns it. It does not save the data.
        /// </remarks>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The entity comparison data.</typeparam>
        /// <param name="context">The context to add seed data to.</param>
        /// <param name="seedData">Data for the given <see cref="TEntity">Entity</see>.</param>
        /// <param name="comparisonProperty">The property to compare.</param>
        /// <returns>The <see cref="TContext"/> with added data.</returns>
        public static TContext AddSeedData<TContext, TEntity, TKey>(this TContext context, IEnumerable<TEntity> seedData, Func<TEntity, TKey> comparisonProperty)
            where TContext : DbContext
            where TEntity : class
        {
            DbSet<TEntity> set = context.Set<TEntity>();

            IEnumerable<TKey> existingKeys = set
                .Select(comparisonProperty);

            List<TEntity> data = seedData.ToList();

            IEnumerable<TEntity> newData = data
                .Where(x => !existingKeys.Contains(comparisonProperty(x)));

            foreach (TEntity entity in newData)
            {
                set.Add(entity);
            }

            return context;
        }

        private static void ExecuteScript<TContext>(this TContext context, string baseSqlScriptDirectory, string sqlScriptFileName)
            where TContext : DbContext
        {
            string script = GetScriptContents(baseSqlScriptDirectory, sqlScriptFileName);

            string separator = $"{Environment.NewLine}GO;";
            string[] scriptParts = script.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in scriptParts)
            {
                context.Database.ExecuteSqlRaw(part);
            }
        }

        private static string GetScriptContents(string baseSqlScriptDirectory, string sqlScriptFileName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string directory = Path.Combine(baseDirectory, baseSqlScriptDirectory);

            string filePath = Path.Combine(directory, sqlScriptFileName);

            string contents = File.ReadAllText(filePath);

            return contents;
        }
    }
}

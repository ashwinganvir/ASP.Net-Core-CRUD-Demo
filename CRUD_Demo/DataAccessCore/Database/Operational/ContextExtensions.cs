using DataAccessCore.Database.Create;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;

namespace DataAccessCore.Database.Operational
{
    /// <summary>
    /// Provide database manipulation functionality as an extension of classes inheriting DbContext.
    /// </summary>
    public static class ContextExtensions
    {
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
        public static TContext SeedData<TContext, TEntity>(this TContext context, IEnumerable<TEntity> seedData)
            where TContext : DbContext
            where TEntity : class
        {
            context.AddSeedData(seedData);

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
        public static TContext SeedData<TContext, TEntity, TKey>(this TContext context, IEnumerable<TEntity> seedData, Func<TEntity, TKey> comparisonProperty)
            where TContext : DbContext
            where TEntity : class
        {
            context.AddSeedData(seedData, comparisonProperty);

            return context;
        }
    }
}

using DataAccessCore.DataAccess.Infrastructure;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DataAccessCore.DataAccess.Context
{
    internal class ContextFactory : IContextFactory
    {
        private readonly DbContextOptionsBuilder _optionsBuilder;

        public ContextFactory(DbContextOptionsBuilder optionsBuilder)
        {
            _optionsBuilder = optionsBuilder;
        }

        public TContext Create<TContext>()
            where TContext : DbContext
        {
            Type[] constructorParams =
            {
                typeof(DbContextOptions)
            };

            ConstructorInfo constructorInfo = typeof(TContext)
                .GetConstructor(constructorParams);
            if (constructorInfo == null)
            {
                throw new Exception($"Constructor not found for {nameof(TContext)}");
            }

            ConstantExpression builder = Expression.Constant(_optionsBuilder.Options);

            Expression[] args =
            {
                builder
            };

            NewExpression body = Expression.New(constructorInfo, args);

            Func<TContext> constructor = Expression.Lambda<Func<TContext>>(body)
                .Compile();

            return constructor();
        }
    }
}

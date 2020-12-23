using CRUD_Demo.Business.Infrastructure;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Threading.Tasks;

namespace CRUD_Demo.Business.Queries
{
    public interface IQueryHandler
    {
        Task<TResult> HandleAsync<TQuery, TResult>(TQuery query)
            where TQuery : class, IQuery;
    }

    internal class QueryHandler : IQueryHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult> HandleAsync<TQuery, TResult>(TQuery query)
            where TQuery : class, IQuery
        {
            var handler = _serviceProvider.GetService<IQueryHandlerAsync<TQuery, TResult>>();

            return await handler.HandleAsync(query);
        }
    }
}

using System.Threading.Tasks;

namespace CRUD_Demo.Business.Infrastructure
{
    internal interface IQueryHandlerAsync<in TQuery, TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}

using System.Threading.Tasks;

namespace CRUD_Demo.Business.Infrastructure
{
    internal interface ICommandHandlerAsync<in TCommand>
        where TCommand : class, ICommand
    {
        Task HandleAsync(TCommand command);
    }

    internal interface ICommandHandlerAsync<in TCommand, TResult>
        where TCommand : class, ICommand
    {
        Task<TResult> HandleAsync(TCommand command);
    }
}

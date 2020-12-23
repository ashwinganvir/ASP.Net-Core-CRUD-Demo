using CRUD_Demo.Business.Infrastructure;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Threading.Tasks;

namespace CRUD_Demo.Business.Commands
{
    public interface ICommandHandler
    {
        Task HandleAsync<TCommand>(TCommand command)
            where TCommand : class, ICommand;

        Task<TResult> HandleAsync<TCommand, TResult>(TCommand command)
            where TCommand : class, ICommand;
    }

    internal class CommandHandler : ICommandHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task HandleAsync<TCommand>(TCommand command)
            where TCommand : class, ICommand
        {
            var handler = _serviceProvider.GetService<ICommandHandlerAsync<TCommand>>();

            await handler.HandleAsync(command);
        }

        public async Task<TResult> HandleAsync<TCommand, TResult>(TCommand command)
            where TCommand : class, ICommand
        {
            var handler = _serviceProvider.GetService<ICommandHandlerAsync<TCommand, TResult>>();

            return await handler.HandleAsync(command);
        }
    }
}

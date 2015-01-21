using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core;

namespace HackedBrain.ServiceBus
{
    public interface ICommand
    {
        string Id
        {
            get;
        }
    }

    public interface ICommandBus
    {
        Task SendCommandAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand;

        Task SendCommandsAsync(IEnumerable<ICommand> commands, CancellationToken cancellationToken);
    }
}

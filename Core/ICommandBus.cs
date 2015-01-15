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
        Task SendCommandAsync(Envelope<ICommand> commandEnvelope, CancellationToken cancellationToken);
        Task SendCommandsAsync(IEnumerable<Envelope<ICommand>> commandEnvelopes, CancellationToken cancellationToken);
    }
}

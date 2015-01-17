using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync(ICommand command, CancellationToken cancellationToken);
    }
}

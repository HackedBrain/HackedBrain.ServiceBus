using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public interface IDispatcher
    {
        Task DispatchAsync(object messageBody, CancellationToken cancellationToken);
    }
}

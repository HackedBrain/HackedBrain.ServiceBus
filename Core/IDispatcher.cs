using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public interface IDispatcher<T>
    {
        Task DispatchAsync(T what, CancellationToken cancellationToken);
    }
}

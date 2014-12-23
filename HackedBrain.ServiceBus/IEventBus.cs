using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus
{
	public interface IEventBus
	{
        Task PublishEventAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken) where TEvent : class;
	}
}

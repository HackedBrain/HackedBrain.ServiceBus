using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus
{
	public interface IEventBus
	{
        Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : class;
	}
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public interface IEvent
    {
        string SourceId
        {
            get;
        }
    }

    public interface IEventBus
    {
        Task PublishEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent;
        Task PublishEventsAsync(IEnumerable<IEvent> events, CancellationToken cancellationToken);
    }
}

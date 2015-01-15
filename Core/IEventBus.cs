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
        Task PublishEventAsync(Envelope<IEvent> eventEnvelope, CancellationToken cancellationToken);
        Task PublishEventsAsync(IEnumerable<Envelope<IEvent>> eventEnvelopes, CancellationToken cancellationToken);
    }
}

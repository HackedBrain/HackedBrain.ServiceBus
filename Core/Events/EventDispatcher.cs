using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
    public class EventDispatcher : Dispatcher<IEvent>
    {
        public EventDispatcher(IHandlerFactory eventHandlerFactory) : base(eventHandlerFactory)
        {
        }
    }
}

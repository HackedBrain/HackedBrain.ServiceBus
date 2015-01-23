using System.Collections.Generic;

namespace HackedBrain.ServiceBus.Core
{
    public interface IEventMessageBuilder
    {
        IMessage<TEvent> BuildMessage<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}

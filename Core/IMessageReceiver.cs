using System;

namespace HackedBrain.ServiceBus.Core
{
    public interface IMessageReceiver
    {
        IObservable<IMessage<TMessageBody>> WhenMessageReceived<TMessageBody>(TimeSpan waitTimeout = default(TimeSpan));
    }
}

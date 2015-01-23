using System;
using System.Reactive.Linq;
using HackedBrain.ServiceBus.Core;
using HackedBrain.WindowsAzure.ServiceBus.Messaging;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
    public class ServiceBusTopicMessageReceiver : IMessageReceiver
    {
        #region Fields

        private MessageReceiver messageReceiver;
        private IMessageBodySerializer messageBodySerializer;

        #endregion

        #region Constructors

        public ServiceBusTopicMessageReceiver(MessageReceiver messageReceiver, IMessageBodySerializer messageBodySerializer)
        {
            this.messageReceiver = messageReceiver;
            this.messageBodySerializer = messageBodySerializer;
        }

        #endregion

        #region IMessageReceiver implementation

        public IObservable<IMessage<TMessageBody>> WhenMessageReceived<TMessageBody>(TimeSpan waitTimeout = default(TimeSpan))
        {
            return this.messageReceiver
                .WhenMessageReceived(waitTimeout)
                .Select(brokeredMessage => brokeredMessage.ToMessage<TMessageBody>(this.messageBodySerializer));
        }

        #endregion
    }
}

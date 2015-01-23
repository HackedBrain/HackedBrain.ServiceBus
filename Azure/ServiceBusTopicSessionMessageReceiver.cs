using System;
using System.Reactive.Linq;
using HackedBrain.ServiceBus.Core;
using HackedBrain.WindowsAzure.ServiceBus.Messaging;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
    public class ServiceBusTopicSessionMessageReceiver : IMessageReceiver
    {
        #region Fields

        private SubscriptionClient subscriptionClient;
        private IMessageBodySerializer messageBodySerializer;

        #endregion

        #region Constructors

        public ServiceBusTopicSessionMessageReceiver(SubscriptionClient subscriptionClient, IMessageBodySerializer messageBodySerializer)
        {
            this.subscriptionClient = subscriptionClient;
            this.messageBodySerializer = messageBodySerializer;
        }

        #endregion

        #region IMessageReceiver implementation

        public IObservable<IMessage<TMessageBody>> WhenMessageReceived<TMessageBody>(TimeSpan waitTimeout = default(TimeSpan))
        {
            return this.subscriptionClient
                .WhenSessionAccepted(waitTimeout)
                .SelectMany(session => session.WhenMessageReceived(waitTimeout)
                                        .Select(brokeredMessage => brokeredMessage.ToMessage<TMessageBody>(this.messageBodySerializer)));
        }

        #endregion
    }
}

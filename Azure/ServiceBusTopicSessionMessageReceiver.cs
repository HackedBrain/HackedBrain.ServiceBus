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

        #endregion

        #region Constructors

        public ServiceBusTopicSessionMessageReceiver(SubscriptionClient subscriptionClient)
        {
            this.subscriptionClient = subscriptionClient;
        }

        #endregion

        #region IMessageReceiver implementation

        public IObservable<IMessage<TMessageBody>> WhenMessageReceived<TMessageBody>(TimeSpan waitTimeout = default(TimeSpan))
        {
            return this.subscriptionClient
                .WhenSessionAccepted(waitTimeout)
                .SelectMany(session => session.WhenMessageReceived(waitTimeout)
                                        .Select(brokeredMessage => brokeredMessage.ToMessage<TMessageBody>()));
        }

        #endregion
    }
}

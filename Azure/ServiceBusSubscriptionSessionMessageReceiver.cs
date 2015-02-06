using System;
using System.Reactive.Linq;
using HackedBrain.ServiceBus.Core;
using HackedBrain.WindowsAzure.ServiceBus.Messaging;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
    public class ServiceBusSubscriptionSessionMessageReceiver : IMessageReceiver
    {
        #region Fields

        private SubscriptionClient subscriptionClient;
        private IMessageBodySerializer messageBodySerializer;

        #endregion

        #region Constructors

        public ServiceBusSubscriptionSessionMessageReceiver(SubscriptionClient subscriptionClient, IMessageBodySerializer messageBodySerializer)
        {
            this.subscriptionClient = subscriptionClient;
            this.messageBodySerializer = messageBodySerializer;
        }

        #endregion

        #region IMessageReceiver implementation

        public IObservable<IMessage> WhenMessageReceived(TimeSpan waitTimeout = default(TimeSpan))
        {
            return this.subscriptionClient
                .WhenSessionAccepted(waitTimeout)
                .SelectMany(session => session.WhenMessageReceived(waitTimeout)
                                        .Select(brokeredMessage => brokeredMessage.ToMessage(this.messageBodySerializer)));
        }

        #endregion
    }
}

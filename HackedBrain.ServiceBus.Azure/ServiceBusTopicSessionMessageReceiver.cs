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

		public IObservable<IMessage> WhenMessageReceived()
		{
			return this.subscriptionClient
				.WhenSessionAccepted()
				.SelectMany(session => session.WhenMessageReceived()
										.Select(brokeredMessage => new BrokeredMessageBasedMessage(brokeredMessage)));
		}

		#endregion
	}
}

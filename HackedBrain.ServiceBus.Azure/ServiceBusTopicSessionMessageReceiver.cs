using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core;
using Microsoft.ServiceBus.Messaging;
using HackedBrain.WindowsAzure.ServiceBus.Messaging;
using System.Reactive.Linq;

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

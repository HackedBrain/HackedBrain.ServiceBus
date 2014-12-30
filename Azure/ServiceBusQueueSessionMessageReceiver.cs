using System;
using System.Reactive.Linq;
using HackedBrain.ServiceBus.Core;
using HackedBrain.WindowsAzure.ServiceBus.Messaging;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
	public class ServiceBusQueueSessionMessageReceiver : IMessageReceiver
	{
		#region Fields

		private QueueClient queueClient;

		#endregion

		#region Constructors

		public ServiceBusQueueSessionMessageReceiver(QueueClient queueClient)
		{
			this.queueClient = queueClient;
		}

		#endregion

		#region IMessageReceiver implementation

		public IObservable<IMessage> WhenMessageReceived(TimeSpan waitTimeout = default(TimeSpan))
		{
			return this.queueClient
				.WhenSessionAccepted()
				.SelectMany(session => session.WhenMessageReceived()
										.Select(brokeredMessage => new BrokeredMessageBasedMessage(brokeredMessage)));
		}

		#endregion
	}
}

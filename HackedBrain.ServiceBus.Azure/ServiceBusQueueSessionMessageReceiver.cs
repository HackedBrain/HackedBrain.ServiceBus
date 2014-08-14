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

		public IObservable<IMessage> WhenMessageReceived()
		{
			return this.queueClient
				.WhenSessionAccepted()
				.SelectMany(session => session.WhenMessageReceived()
										.Select(brokeredMessage => new BrokeredMessageBasedMessage(brokeredMessage)));
		}

		#endregion
	}
}

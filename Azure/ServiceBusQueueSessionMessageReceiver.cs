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
        private IMessageBodySerializer messageBodySerializer;

		#endregion

		#region Constructors

		public ServiceBusQueueSessionMessageReceiver(QueueClient queueClient, IMessageBodySerializer messageBodySerializer)
		{
			this.queueClient = queueClient;
            this.messageBodySerializer = messageBodySerializer;
		}

		#endregion

		#region IMessageReceiver implementation

		public IObservable<IMessage<TMessageBody>> WhenMessageReceived<TMessageBody>(TimeSpan waitTimeout = default(TimeSpan))
		{
			return this.queueClient
				.WhenSessionAccepted()
				.SelectMany(session => session.WhenMessageReceived()
										.Select(brokeredMessage => brokeredMessage.ToMessage<TMessageBody>(this.messageBodySerializer)));
		}

		#endregion
	}
}

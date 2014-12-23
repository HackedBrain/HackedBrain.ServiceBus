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

		#endregion

		#region Constructors

		public ServiceBusTopicMessageReceiver(MessageReceiver messageReceiver)
		{
			this.messageReceiver = messageReceiver;
		}

		#endregion

		#region IMessageReceiver implementation

		public IObservable<IMessage> WhenMessageReceived()
		{
			return this.messageReceiver
				.WhenMessageReceived()
				.Select(brokeredMessage => new BrokeredMessageBasedMessage(brokeredMessage));
		}

		#endregion
	}
}

﻿using System;
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

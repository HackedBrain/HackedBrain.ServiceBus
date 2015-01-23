using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
	public class ServiceBusQueueMessageSender : ServiceBusMessageClientEntityMessageSender
	{
		#region Constructors

		public ServiceBusQueueMessageSender(QueueClient queueClient, IMessageBodySerializer messageBodySerializer) : base(queueClient, queueClient.SendAsync, messageBodySerializer)
		{
		}

		#endregion
	}
}

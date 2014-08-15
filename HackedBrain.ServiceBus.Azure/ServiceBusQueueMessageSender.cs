using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
	public class ServiceBusQueueMessageSender : IMessageSender
	{
		#region Fields

		private QueueClient queueClient;		

		#endregion

		#region Constructors

		public ServiceBusQueueMessageSender(QueueClient queueClient)
		{
			this.queueClient = queueClient;
		}

		#endregion

		#region IMessageSender implementation

		public Task SendAsync<TMessageBody>(TMessageBody body, IEnumerable<KeyValuePair<string, object>> metadata) where TMessageBody : class
		{
			BrokeredMessage brokeredMessage = new BrokeredMessage(body);

			foreach(KeyValuePair<string, object> entry in metadata)
			{
				brokeredMessage.Properties.Add(entry);
			}

			return this.queueClient.SendAsync(brokeredMessage);
		}

		#endregion
	}
}

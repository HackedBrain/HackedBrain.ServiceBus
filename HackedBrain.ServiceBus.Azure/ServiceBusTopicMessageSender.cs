using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
	public class ServiceBusTopicMessageSender : IMessageSender
	{
		#region Fields

		private TopicClient topicClient;		

		#endregion

		#region Constructors

		public ServiceBusTopicMessageSender(TopicClient topicClient)
		{
			this.topicClient = topicClient;
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

			return this.topicClient.SendAsync(brokeredMessage);
		}

		#endregion
	}
}

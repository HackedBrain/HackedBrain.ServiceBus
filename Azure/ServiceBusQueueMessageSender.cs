using System;
using System.Collections.Generic;
using System.Threading;
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

        public Task SendAsync<TMessageBody>(TMessageBody body, IEnumerable<KeyValuePair<string, object>> metadata, CancellationToken cancellationToken) where TMessageBody : class
		{
			BrokeredMessage brokeredMessage = new BrokeredMessage(body);
            brokeredMessage.MessageId = Guid.NewGuid().ToString("N");

            IMessageSessionIdProvider messageSessionIdProvider = body as IMessageSessionIdProvider;

            if(messageSessionIdProvider != null)
            {
                brokeredMessage.SessionId = messageSessionIdProvider.SessionId;
            }

			foreach(KeyValuePair<string, object> entry in metadata)
			{
				brokeredMessage.Properties.Add(entry);
			}

            cancellationToken.ThrowIfCancellationRequested();

			return this.queueClient.SendAsync(brokeredMessage);
		}

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackedBrain.ServiceBus.Core;
using Microsoft.ServiceBus.Messaging;

namespace HackedBrain.ServiceBus.Azure
{
	internal sealed class BrokeredMessageBasedMessage : IMessage
	{
		#region Fields

		private BrokeredMessage brokeredMessage;

		#endregion

		#region Constructors

		public BrokeredMessageBasedMessage(BrokeredMessage brokeredMessage)
		{
			this.brokeredMessage = brokeredMessage;
		}

		#endregion

		#region IMessage implementation

        public Guid Id
        {
            get
            {
                return new Guid(this.brokeredMessage.MessageId);
            }
        }

        public string SessionId
        {
            get
            {
                return this.brokeredMessage.SessionId;
            }
        }

        public string CorrelationId
        {
            get
            {
                return this.brokeredMessage.CorrelationId;
            }
        }

		public IEnumerable<KeyValuePair<string, object>> Metadata
		{
			get
			{
				return this.brokeredMessage.Properties.Select(kvp => kvp);
			}
		}

		public T GetBody<T>() where T : class
		{
			return this.brokeredMessage.GetBody<T>();
		}

		public Task CompleteAsync()
		{

			return this.brokeredMessage.CompleteAsync();
		}

        public Task AbandonAsync()
		{
			return this.brokeredMessage.AbandonAsync();
		}

		public Task QuarantineAsync(string reason, string description)
		{
			return this.brokeredMessage.DeadLetterAsync(reason, description);
		}

		#endregion
	}
}

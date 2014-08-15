using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public void Complete()
		{
			this.brokeredMessage.Complete();
		}
		
		public Task CompleteAsync()
		{

			return this.brokeredMessage.CompleteAsync();
		}

		public void Abandon()
		{
			this.brokeredMessage.Abandon();
		}

		public Task AbandonAsync()
		{
			return this.brokeredMessage.AbandonAsync();
		}

		public void Quarantine(string reason, string description)
		{
			this.brokeredMessage.DeadLetter(reason, description);
		}

		public Task QuarantineAsync(string reason, string description)
		{
			return this.brokeredMessage.DeadLetterAsync(reason, description);
		}


		#endregion
	}
}

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

		public IDictionary<string, object> Metadata
		{
			get
			{
				return this.brokeredMessage.Properties;
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


		#endregion
	}
}

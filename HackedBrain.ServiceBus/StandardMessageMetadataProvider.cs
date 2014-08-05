using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
	public class StandardMessageMetadataProvider : IMessageMetadataProvider
	{
		private static readonly string MessageTypeKey = "StandardMessageMetadataProvider.MessageType";
		private static readonly string CreatedOnKey = "StandardMessageMetadataProvider.CreatedOn";
		private static readonly string ProviderVersionKey = "StandardMessageMetadataProvider.ProviderVersion";

		private static readonly string ProviderVersionValue = typeof(StandardMessageMetadataProvider).Assembly.GetName().Version.ToString();



		#region IMessageMetadataProvider implementation

		public Func<IDictionary<string, object>, bool> GenerateMessageTypeFilter<TMessageType>()
		{
			return message => message[StandardMessageMetadataProvider.MessageTypeKey].Equals(typeof(TMessageType).Name);
		}

		public void ApplyMetadataToMessage<TMessageType>(IDictionary<string, object> serializedMessage)
		{
			serializedMessage.Add(StandardMessageMetadataProvider.ProviderVersionKey, StandardMessageMetadataProvider.ProviderVersionValue);
			serializedMessage.Add(StandardMessageMetadataProvider.MessageTypeKey, typeof(TMessageType).Name);
			serializedMessage.Add(StandardMessageMetadataProvider.CreatedOnKey, DateTime.UtcNow);			
		}

		#endregion
	}
}

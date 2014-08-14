using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
	public class StandardMessageMetadataProvider : IMessageMetadataProvider
	{
		#region Fields

		private static readonly string MessageTypeKey = "StandardMessageMetadataProvider.MessageType";
		private static readonly string CreatedOnKey = "StandardMessageMetadataProvider.CreatedOn";
		private static readonly string ProviderVersionKey = "StandardMessageMetadataProvider.ProviderVersion";

		private static readonly string ProviderVersionValue = typeof(StandardMessageMetadataProvider).Assembly.GetName().Version.ToString();

		#endregion

		#region IMessageMetadataProvider implementation

		public Func<IDictionary<string, object>, bool> GenerateMessageTypeFilter<TMessage>() where TMessage : class
		{
			return metadata => metadata[StandardMessageMetadataProvider.MessageTypeKey].Equals(typeof(TMessage).Name);
		}

		public IEnumerable<KeyValuePair<string, object>> GenerateMetadata<TMessage>(TMessage message) where TMessage : class
		{
			return new KeyValuePair<string, object>[]
			{
				{ StandardMessageMetadataProvider.ProviderVersionKey, StandardMessageMetadataProvider.ProviderVersionValue },
				{ StandardMessageMetadataProvider.MessageTypeKey, typeof(TMessage).Name },
				{ StandardMessageMetadataProvider.CreatedOnKey, DateTime.UtcNow },
			};
		}

		#endregion
	}
}

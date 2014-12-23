using System;
using System.Collections.Generic;

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

		public IEnumerable<KeyValuePair<string, object>> GenerateMetadata<TMessage>(TMessage message) where TMessage : class
		{
			return new KeyValuePair<string, object>[]
			{
				new KeyValuePair<string, object>(StandardMessageMetadataProvider.ProviderVersionKey, StandardMessageMetadataProvider.ProviderVersionValue),
				new KeyValuePair<string, object>(StandardMessageMetadataProvider.MessageTypeKey, typeof(TMessage).FullName),
				new KeyValuePair<string, object>(StandardMessageMetadataProvider.CreatedOnKey, DateTime.UtcNow),
			};
		}

		#endregion
	}
}

using System.Collections.Generic;

namespace HackedBrain.ServiceBus.Core
{
	public interface IMessageMetadataProvider
	{
		IEnumerable<KeyValuePair<string, object>> GenerateMetadata<TMessageType>(TMessageType message)  where TMessageType : class;
	}
}

using System.Collections.Generic;

namespace HackedBrain.ServiceBus.Core
{
	public interface IMessageMetadataProvider
	{
		IEnumerable<KeyValuePair<string, object>> GenerateMetadata<TBody>(TBody body);
	}
}

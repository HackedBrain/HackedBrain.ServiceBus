using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackedBrain.ServiceBus.Core
{
	public interface IMessageMetadataProvider
	{
		Func<IDictionary<string, object>, bool> GenerateMessageTypeFilter<TMessageType>();

		void ApplyMetadataToMessage<TMessageType>(IDictionary<string, object> serializedMessage);
	}
}

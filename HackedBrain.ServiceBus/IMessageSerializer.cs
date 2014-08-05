using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackedBrain.ServiceBus.Core
{
	public interface IMessageSerializer
	{
		IDictionary<string, object> Serialize<TMessage>(TMessage message);

		TMessage Deserialize<TMessage>(IDictionary<string, object> serializedMessage);
	}
}

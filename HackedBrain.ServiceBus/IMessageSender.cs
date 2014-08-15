using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackedBrain.ServiceBus.Core
{
	public interface IMessageSender
	{
		Task SendAsync<TMessageBody>(TMessageBody body, IEnumerable<KeyValuePair<string, object>> metadata) where TMessageBody : class;
	}
}
